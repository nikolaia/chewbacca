namespace Infrastructure.ApiClients.DTOs;

public class VibesEmploymentDTO
{
    public VibesEmploymentDTO(string email, DateTime? startDate, DateTime? endDate)
    {
        this.email = email;
        this.startDate = startDate ?? new DateTime(2018,08,01);
        this.endDate = endDate;
    }

    public string email { get; set; }
    public DateTime startDate { get; set; }
    public DateTime? endDate { get; set; }
}
