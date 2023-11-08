namespace ApplicationCore.Models;
public record ProjectExprienceResponse{
    public required List<ProjectExperience> projects {get; init;}
    public required int MonthsOfExperience {get; init;}
}