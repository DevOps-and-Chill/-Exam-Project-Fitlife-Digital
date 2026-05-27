using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace ClassServiceAPI.Extensions;

public static class VaultConfigurationExtensions
{
    public static async Task LoadVault(
        this WebApplicationBuilder builder)
    {
        var token =
            builder.Configuration["VAULT_TOKEN"];

        var auth =
            new TokenAuthMethodInfo(
                token);

        var client =
            new VaultClient(
                new VaultClientSettings(
                    builder.Configuration["Vault:Address"],
                    auth));

        var secret =
            await client
                .V1
                .Secrets
                .KeyValue
                .V2
                .ReadSecretAsync(
                    mountPoint: "secret",
                    path: "fitlife");

        builder.Configuration
            .AddInMemoryCollection(
                secret.Data.Data
                    .ToDictionary(
                        x => x.Key.Replace("__", ":"),
                        x => x.Value?.ToString()));
    }
}