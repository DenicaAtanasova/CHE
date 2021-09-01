namespace CHE.Web.Infrastructure
{
    using Azure.Extensions.AspNetCore.Configuration.Secrets;
    using Azure.Identity;
    using Azure.Security.KeyVault.Secrets;

    using Microsoft.Extensions.Configuration;

    using System;

    public static class ConfigurationBuilderExtensions
    {
        public static void ConfigureKeyVault(this IConfigurationBuilder config)
        {
            var builtConfig = config.Build();

            var keyVaultEndpoint = Environment
                .GetEnvironmentVariable(builtConfig["AzureKeyVault:Endpoint"]);
            if (keyVaultEndpoint == null)
            {
                throw new InvalidOperationException($"Store the Key vault endpoint in a KeyVaultEndpoint environment variable!");
            }

            var vaultUri = new Uri(keyVaultEndpoint);
            var secretClient = new SecretClient(vaultUri, new DefaultAzureCredential());

            config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
        }
    }
}