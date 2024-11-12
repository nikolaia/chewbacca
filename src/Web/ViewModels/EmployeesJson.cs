namespace Web.ViewModels;

public record EmployeesJson
{
    public required IEnumerable<EmployeeJson> Employees { get; init; }
};

public record EmployeeJson
{
    public required string Email { get; init; }
    public required string Name { get; set; }
    public string? Telephone { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageThumbUrl { get; set; }
    public required string OfficeName { get; set; }
    public DateTime StartDate { get; set; }
    public required IEnumerable<string> Competences { get; init; }
}