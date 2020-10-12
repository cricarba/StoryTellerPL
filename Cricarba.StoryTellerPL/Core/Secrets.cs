using System;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Cricarba.StoryTellerPL.Core
{
    public class Secrets
    {
        public string GetSecrects(string name)
        {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                        {
                            Delay= TimeSpan.FromSeconds(2),
                            MaxDelay = TimeSpan.FromSeconds(16),
                            MaxRetries = 5,
                            Mode = RetryMode.Exponential
                         }
            };

            var kvUri = $"https://{Environment.GetEnvironmentVariable("KEY_VAULT_NAME")}.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential(), options);
            KeyVaultSecret secret = client.GetSecret(name);
            return secret.Value;
        }
    }
}
