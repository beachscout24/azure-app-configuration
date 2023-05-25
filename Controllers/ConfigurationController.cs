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
        var keyVaultURL = _configuration.GetSection("KeyVault:KeyVaultURL");
        var keyVaultClientId = _configuration.GetSection("KeyVault:ClientId");
        var keyVaultClientSecret = _configuration.GetSection("KeyVault:ClientSecret");
        var keyVaultDirectoryID = _configuration.GetSection("KeyVault:DirectoryID");

        var credential = new ClientSecretCredential(keyVaultDirectoryID.Value!.ToString(), keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value!.ToString());
        var client = new SecretClient(new Uri(keyVaultURL.Value!.ToString()), credential);

        var secret = client.GetSecret("VictoriasSecret").Value.Value.ToString();

        return secret;
    }
}
