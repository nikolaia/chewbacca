namespace Shared;

public record Healthcheck {
    public bool KeyVault { get; set; } 
    public bool AppConfig { get; set; } 
}

public record CvPartnerConfig
{
    public Uri Uri { get; set; } = null!;
    public string Token { get; set; } = null!;
}

public  record BlobStorageConfig
{
    public Uri Endpoint { get; set; } = null!;
    public bool UseDevelopmentStorage { get; set; } = false;
}

public record AppSettings
{
    public Uri AzureAppConfigUri { get; set; } = null!;
    public bool UseAzureAppConfig { get; set; }
    public Healthcheck Healthcheck { get; set; } = null!;
    public CvPartnerConfig CvPartner { get; set; } = null!;
    public string BemanningConnectionString { get; set; } = null!;
    public BlobStorageConfig BlobStorage { get; set; } = null!;
}