namespace Employees.Models;

public record EmployeeInformation
{
    public string Phone { get; init; } = null!;
    public string AccountNr { get; init; } = null!;
    public string Adress { get; init; } = null!;
    public string ZipCode { get; init; } = null!;
    public string City { get; init; } = null!;
}