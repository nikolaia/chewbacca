using System.Globalization;

using Microsoft.Extensions.Options;

using Npgsql;

using Shared;

namespace Bemanning;

public class BemanningRepository : IBemanningRepository
{
    private readonly IOptionsSnapshot<AppSettings> _appSettings;

    public BemanningRepository(IOptionsSnapshot<AppSettings> appSettings)
    {
        _appSettings = appSettings;
    }

    /**
     * <summary>Returns a email and a start date</summary>
     */
    public async Task<List<BemanningEmployee>> GetBemanningDataForEmployees()
    {
        await using var dataSource = NpgsqlDataSource.Create(_appSettings.Value.BemanningConnectionString);

        const string bemanningCommand = """"
           SELECT c."Email" as email, MAX(s."YearWeek") as "startWeek" 
           FROM "Consultant" as c
           LEFT JOIN "Staffing" s ON c.id = s."ConsultantId" 
           WHERE s."Hours" <> 0 AND s."EngagementId" = '6b402b81-44c7-40d2-8a89-3d2c7a57b777' AND c."EndDate" IS NULL
           GROUP BY c.id
           """";

        // Retrieve all rows
        await using NpgsqlCommand cmd = dataSource.CreateCommand(bemanningCommand);
        await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

        List<BemanningEmployee> bemanningEmployees = new();
        while (await reader.ReadAsync())
        {
            bemanningEmployees.Add(
                new BemanningEmployee(reader.GetString(reader.GetOrdinal("email")),
                    FirstDateOfWeekISO8601(reader.GetInt32(reader.GetOrdinal("startWeek")))));
        }

        return bemanningEmployees;
    }

    /**
     * <summary>Calculate and returns the start date</summary>
     * <param name="weekOfYear">String containing year and start week. on the form yyyyww</param>
     */
    private static DateTime FirstDateOfWeekISO8601(int weekOfYear)
    {
        int year = weekOfYear / 100;
        int week = weekOfYear % year;
        // https://stackoverflow.com/questions/662379/calculate-date-from-week-number
        DateTime jan1 = new(year, month: 1, day: 1);
        int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

        // Use first Thursday in January to get first week of the year as
        // it will never be in Week 52/53
        DateTime firstThursday = jan1.AddDays(daysOffset);
        var cal = CultureInfo.CurrentCulture.Calendar;
        int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        // Add 1 so it starts on the first week after last "inactive" day
        int weekNum = week + 1;
        // As we're adding days to a date in Week 1,
        // we need to subtract 1 in order to get the right date for week #1
        if (firstWeek == 1)
        {
            weekNum -= 1;
        }

        // Using the first Thursday as starting week ensures that we are starting in the right year
        // then we add number of weeks multiplied with days
        var result = firstThursday.AddDays(weekNum * 7);

        // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
        return result.AddDays(-3);
    }
}

/**
 * <param name="Email">Users email</param>
 * <param name="StartDate">Users start date</param>
 */
public record BemanningEmployee(string Email, DateTime StartDate);