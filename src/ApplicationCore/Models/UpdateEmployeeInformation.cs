namespace ApplicationCore.Models;

public record UpdateEmployeeInformation
{
    public string? accountNumber { get; init; }
    public string? address { get; init; }
    public string? phone { get; init; }
    public string? zipCode { get; init; }
    public string? city { get; init; }
}