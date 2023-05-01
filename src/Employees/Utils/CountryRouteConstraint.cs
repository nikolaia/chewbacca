using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public class CountryRouteConstraint : IRouteConstraint
{
    private readonly string[] _availableCountries = new[] { "no", "se" };

    public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (values.TryGetValue("country", out var country) && country != null)
        {
            return _availableCountries.Contains(country.ToString()!.ToLower());
        }

        return false;
    }
}
