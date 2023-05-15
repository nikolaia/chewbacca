

using Microsoft.Extensions.Options;

using Shared;

public class FilteredUids
{

    private readonly IOptionsSnapshot<AppSettings> _appSettings;

    public FilteredUids(
        IOptionsSnapshot<AppSettings> appSettings)
    {
        _appSettings = appSettings;
    }

    public IList<string> Uids => string.IsNullOrEmpty(_appSettings?.Value?.FilteredUids)
        ? new string[] { }
        : _appSettings.Value.FilteredUids.Split(',');
}