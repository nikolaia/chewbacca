using Microsoft.AspNetCore.Mvc;

using Scheduler.Service;

namespace Scheduler;

[ApiController]
[Route("[controller]")]
public class SchedulerController
{
    private readonly SchedulerService _service;


    public SchedulerController(SchedulerService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task TestScheduler()
    {
        await _service.AddEmployeeToDatabase();
    }
}