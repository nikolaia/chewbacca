namespace Shared;

public record HealthcheckResponse
{
    public bool Database { get; set; }
    public bool KeyVault { get; set; }
    public bool AppConfig { get; set; }
}