namespace ApplicationCore.Models;

public class Cv
{
    public required string Email { get; init; }
    public List<WorkExperience> WorkExperiences { get; init; } = new();
    public List<ProjectExperience> ProjectExperiences { get; init; } = new();
    public List<Presentation> Presentations { get; init; } = new();

    public List<Certification> Certifiactions { get; init; } = new();
}

public class WorkExperience
{
    public required string Id { get; init; }
    public required string Title { get; init; } 
    public required string Description { get; init; }
    public string? MonthFrom { get; init; }
    public string? YearFrom { get; init; }
    public string? MonthTo { get; init; }
    public string? YearTo { get; init; }
}

public class ProjectExperience
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string? MonthFrom { get; init; }
    public string? YearFrom { get; init; }
    public string? MonthTo { get; init; }
    public string? YearTo { get; init; }

    public List<ProjectExperienceRole> Roles { get; init; } = new();
}

public class ProjectExperienceRole
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
}

public class Presentation
{
    public required string Id { get; init; }
    public  required string Title { get; init; }
    public required string Description { get; init; }
    public string? Month { get; init; }
    public string? Year { get; init; }
}

public class Certification
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public DateTime? ExpiryDate { get; init; }
    public string? IssuedMonth { get; init; }
    public string? IssuedYear { get; init; }
}