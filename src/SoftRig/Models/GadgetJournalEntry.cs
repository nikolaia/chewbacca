namespace SoftRig.Models;

public record GadgetJournalEntry
{
    // TODO: need to set UniEconomyId, CompanyKey and Employee manually
    public string UniEconomyId { get; init; } = null!;
    public string CompanyKey { get; init; } = null!;
    public string JournalEntryNumber { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Employee { get; init; } = null!;
    public string FinancialDate { get; init; } = null!;
    public double Amount { get; init; }
    public int AccountNumber { get; init; }
    public string AccountName { get; init; } = null!;
    public string EmployeeName { get; init; } = null!;
}