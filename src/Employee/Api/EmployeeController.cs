using CvPartner.Api;
using Employee.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared;

namespace Employee.Api;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    public EmployeeController()
    {
    }

    [HttpGet]
    public List<Models.Employee> Get()
    {
        // Hente Employees fra EmployeesSevice
        // Returner
        throw new NotImplementedException();
    }



}