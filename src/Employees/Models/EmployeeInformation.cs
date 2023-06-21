namespace Employees.Models;

public record EmployeeInformation
{
    public string? Phone { get; init; }
    public string? AccountNumber { get; init; }
    public string? Address { get; init; }
    public string? ZipCode { get; init; }
    public string? City { get; init; }
}