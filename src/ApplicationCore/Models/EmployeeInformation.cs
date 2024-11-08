namespace ApplicationCore.Models;

public record EmployeeInformation
{
    public required string Email { get; init; }
    public required string Name { get; init; }
    public string? Telephone { get; init; }
    public string? ImageUrl { get; init; }
    public string? ImageThumbUrl { get; init; }
    public required string OfficeName { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; set; }
    public string? AccountNumber { get; init; }
    public string? Address { get; init; }
    public string? ZipCode { get; init; }
    public string? City { get; init; }
    public required string CountryCode { get; set; }
    public List<string> Competences { get; set; } = [];
}