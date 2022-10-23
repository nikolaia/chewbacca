using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BlogController : ControllerBase
{
    private readonly BloggingContext _db;
    private readonly ILogger<BlogController> _logger;

    public BlogController(BloggingContext db, ILogger<BlogController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpGet]
    public List<Blog> Get()
    {
        _logger.LogInformation("Getting blog from database");
        
        return _db.Blogs.ToList();
    }
}
