using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

public class CountryContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CountryContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Country => _httpContextAccessor.HttpContext?.GetRouteValue("country")?.ToString();
}
