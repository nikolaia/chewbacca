namespace ApplicationCore.Entities;

/**
 * <param name="Email">Users email</param>
 * <param name="StartDate">Users start date</param>
 */
public record BemanningEmployee(string Email, DateTime StartDate, DateTime? EndDate);