using System.Globalization;

using Invoicing;
using Invoicing.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApi.Pages;

public class Fakturagrunnlag : PageModel
{
    public HarvestService.Office Office { get; set; }
    public MappedProject? Project { get; set; }
    public string? MonthReadable { get; set; }
    public List<DateTimeOffset> WeekendDates { get; set; }

    public List<DateTimeOffset> Dates { get; set; }

    public async Task<IActionResult> OnGetAsync([FromQuery] HarvestService.Office office,
        [FromServices] HarvestService harvestService)
    {
        if (office is not (HarvestService.Office.Trondheim or HarvestService.Office.Oslo
            or HarvestService.Office.Bergen))
        {
            return BadRequest();
        }

        var config = harvestService.GetHarvestConfig(office);

        if (config == null)
        {
            return StatusCode(500);
        }

        var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
        var toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

        var projects = await harvestService.GetAllMappedEntries(config, fromDate, toDate);

        MonthReadable = fromDate.ToString("MMMM yyyy", new CultureInfo("nb-NO"));
        // List of DateTimeOffsets for every date in the month
        Dates = Enumerable.Range(1, DateTime.DaysInMonth(fromDate.Year, fromDate.Month))
            .Select(day => new DateTimeOffset(fromDate.Year, fromDate.Month, day, 0, 0, 0, TimeSpan.Zero))
            .ToList();
        // Dates that are the weekend
        WeekendDates = Dates.Where(date => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday).ToList();
        Project = projects.First();

        return Page();
    }

}