using Microsoft.AspNetCore.Mvc;

namespace Bemanning.Api;

[ApiController]
[Route("[controller]")]
public class BemanningController
{
    private readonly BemanningRepository _bemanningRepository;

    public BemanningController(BemanningRepository bemanningRepository)
    {
        _bemanningRepository = bemanningRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<BemanningEmployee>> Get()
    {
        return await _bemanningRepository.GetBemanningDataForEmployees();
    }
}