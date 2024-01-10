using MicroBlog.Models;
using MicroBlog.Services;
using MicroBlog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroBlog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ElasticSearchController : ControllerBase
{
    private readonly IElasticSearchService _elasticSearchService;

    public ElasticSearchController(IElasticSearchService elasticSearchService)
    {
        _elasticSearchService = elasticSearchService;
    }

    [HttpGet]
    public async Task<IEnumerable<MessageElastic>> Get()
    {
        return await _elasticSearchService.GetLimitedAsync();
    }

    [HttpGet("{limit:int}")]
    public async Task<IEnumerable<MessageElastic>> Get(int limit)
    {
        return await _elasticSearchService.GetLimitedAsync(limit);
    }
    
    [HttpGet("{year:int}-{month:int}-{day:int}")]
    public async Task<IEnumerable<MessageElastic>> FindMessagesByDay(
        int year, int month, int day)
    {
        return await _elasticSearchService.FindMessagesByDayAsync(year, month, day);
    }
  
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<MessageElastic>> Get(string id)
    {
        var message = await _elasticSearchService.GetAsync(id);

        if (message is null)
        {
            return NotFound();
        }

        return message;
    }
}