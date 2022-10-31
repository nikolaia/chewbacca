namespace Shared;

public record Healthcheck {
    public bool KeyVault { get; set; } 
    public bool AppConfig { get; set; } 
}

public record AppSettings
{
    public Uri AzureAppConfigUri { get; set; }
    public bool UseAzureAppConfig { get; set; }
    public Healthcheck Healthcheck { get; set; }
    public string Token { get; set; }
}