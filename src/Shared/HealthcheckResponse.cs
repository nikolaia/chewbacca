namespace Shared;

public record HealthcheckResponse
{
    public bool Database { get; init; }
    public bool KeyVault { get; init; }
    public bool AppConfig { get; init; }
    public bool FeatureManagement { get; set; }
}