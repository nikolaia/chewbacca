namespace Infrastructure.ApiClients.DTOs;

public class VibesConsultantDTO
{
    public required string Email { get; set; }
    public required List<CompetenceDTO> Competences { get; set; }
}

public class CompetenceDTO
{
    public required string Id { get; set; }
    public required string Name { get; set; }
}