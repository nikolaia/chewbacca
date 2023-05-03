namespace Shared;

public record Healthcheck
{
    public bool KeyVault { get; set; }
    public bool AppConfig { get; set; }
}

public record CvPartnerConfig
{
    public Uri Uri { get; set; } = null!;
    public string Token { get; set; } = null!;
}

public record BlobStorageConfig
{
    public Uri Endpoint { get; set; } = null!;
    public bool UseDevelopmentStorage { get; set; } = false;
}

public record InvoicingConfig
{
    public Uri Uri { get; set; }
    public HarvestConfig Oslo { get; set; }
    public HarvestConfig Trondheim { get; set; }
    public HarvestConfig Bergen { get; set; }
}

public record HarvestConfig
{
    public string AccountId { get; set; }
    public string AccessToken { get; set; }
}

public record AppSettings
{
    public Uri AzureAppConfigUri { get; set; } = null!;
    public bool UseAzureAppConfig { get; set; }
    public Healthcheck Healthcheck { get; set; } = null!;
    public CvPartnerConfig CvPartner { get; set; } = null!;
    public string FilteredUids { get; set; } = null!;
    public string BemanningConnectionString { get; set; } = null!;
    public BlobStorageConfig BlobStorage { get; set; } = null!;
    public InvoicingConfig Invoicing { get; set; }
}