using Scheduler.Repositories;

namespace Scheduler.Service;

public class SchedulerService
{
    private readonly SchedulerRepository _schedulerRepository;

    public SchedulerService(SchedulerRepository schedulerRepository)
    {
        _schedulerRepository = schedulerRepository;
    }

    public async Task AddEmployeeToDatabase()
    {
        await _schedulerRepository.postEmployee();
    }
}