namespace Employees.Models;

public class Cv
{
    public List<WorkExperience> WorkExperiences { get; init; } = new();
    public List<ProjectExperience> ProjectExperiences { get; init; } = new();
    public List<Presentation> Presentations { get; init; } = new();
}

public class WorkExperience
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? MonthFrom { get; set; }
    public string? YearFrom { get; set; }
    public string? MonthTo { get; set; }
    public string? YearTo { get; set; }
}

public class ProjectExperience
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? MonthFrom { get; set; }
    public string? YearFrom { get; set; }
    public string? MonthTo { get; set; }
    public string? YearTo { get; set; }
}

public class Presentation
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Month { get; set; }
    public string? Year{ get; set; }
}