
using Microsoft.AspNetCore.Mvc;
namespace Bemanning.Api;

public interface IBemanningApi
{
    [HttpGet]
    Task<IEnumerable<BemanningEmployee>> Get();
}