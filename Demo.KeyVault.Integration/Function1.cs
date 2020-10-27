using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace Demo.KeyVault.Integration
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            string keyVaultName = Environment.GetEnvironmentVariable("KeyVaultName");
            var kvUri = "https://" + keyVaultName + ".vault.azure.net";

            string name = req.Query["name"];
            var client = new SecretClient(new Uri(kvUri), new ClientSecretCredential(Environment.GetEnvironmentVariable("TenantId"), Environment.GetEnvironmentVariable("ClientId"), Environment.GetEnvironmentVariable("ClientSecret")));
            var secret = await client.GetSecretAsync(name);

            return new OkObjectResult(secret);
        }
    }
}
