namespace ApplicationCore.Models;

public record Employee
{
    public string Email { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? Telephone { get; init; }
    public string? ImageUrl { get; init; }
    public string OfficeName { get; init; } = null!;
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; set; }
    public string? AccountNumber { get; init; }
    public string? Address { get; init; }
    public string? ZipCode { get; init; }
    public string? City { get; init; }
    public string CountryCode { get; set; } = null!;
}