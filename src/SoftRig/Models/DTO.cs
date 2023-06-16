namespace SoftRig.Models;

// TODO: 
// Hente gadgetbudsjett, lønnskjøring
// Hente nvn, epost, adresse, kontonr?

public record GadgetJournalEntry
{
    public string UniEconomyId { get; init; } = null!;
    public string CompanyKey { get; init; } = null!;
    public string JournalEntryNumber { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Employee { get; init; } = null!;
    public int? Amount { get; init; } = null!;
    public string FinancialDate { get; init; } = null!;
    public int? AccountNumber { get; init; } = null!;
    public string AccountName { get; init; } = null!;
}