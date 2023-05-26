using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Options;

namespace azure_app_configuration.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigurationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private Settings.Settings Settings { get; }

    public ConfigurationController(IConfiguration configuration, IOptionsSnapshot<Settings.Settings> options)
    {
        _configuration = configuration;
        Settings = options.Value;
    }

    [HttpGet]
    public IEnumerable<string> GetConfiguration()
    {
        var strings = new List<string>();
        strings.Add(Settings.FirstConfig!);
        strings.Add(Settings.SecondConfig!);
        strings.Add(GetSecret());
        return strings;
    }

    private string GetSecret()
    {
        var keyVaultURL = Settings.KeyVaultURL;
        var keyVaultClientId = Settings.ClientId;
        var keyVaultClientSecret = Settings.ClientSecret;
        var keyVaultDirectoryID = Settings.DirectoryID;

        var credential = new ClientSecretCredential(keyVaultDirectoryID, keyVaultClientId, keyVaultClientSecret);
        var client = new SecretClient(new Uri(keyVaultURL), credential);

        var secret = client.GetSecret("VictoriasSecret").Value.Value.ToString();

        return secret;
    }
}
