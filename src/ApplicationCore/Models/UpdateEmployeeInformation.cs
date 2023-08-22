namespace ApplicationCore.Models;

public record UpdateEmployeeInformation
{
    public string? AccountNumber { get; init; }
    public string? Address { get; init; }
    public string? Phone { get; init; }
    public string? ZipCode { get; init; }
    public string? City { get; init; }
}