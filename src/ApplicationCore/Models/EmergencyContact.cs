namespace ApplicationCore.Models;

public record EmergencyContact
{
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Relation { get; set; }
    public string? Comment { get; set; }
}