namespace Shared;

public record Healthcheck {
    public bool KeyVault { get; set; } 
    public bool AppConfig { get; set; } 
}

public record CvPartnerConfig
{
    public Uri Uri { get; set; }
    public string Token { get; set; }
}

public record BlobStorageConfig
{
    public Uri Endpoint { get; set; }
    public bool UseDevelopmentStorage { get; set; } = false;
}

public record AppSettings
{
    public Uri AzureAppConfigUri { get; set; }
    public bool UseAzureAppConfig { get; set; }
    public Healthcheck Healthcheck { get; set; }
    public CvPartnerConfig CvPartner { get; set; }
    public string BemanningConnectionString { get; set; }
    public BlobStorageConfig BlobStorage { get; set; }
}