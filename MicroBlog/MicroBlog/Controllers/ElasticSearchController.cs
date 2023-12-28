using MicroBlog.Models;
using MicroBlog.Services;
using MicroBlog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroBlog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ElasticSearchController : ControllerBase
{
    private readonly ElasticSearchService _elasticSearchService;

    public ElasticSearchController(ElasticSearchService elasticSearchService)
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

    /*
    [HttpGet("total-count")]
    public async Task<long> GetTotalCount()
    {
        var count = await _elasticSearchService.GetTotalCountAsync();
        
        return count;
    }
    */
    /*
    [HttpGet("{query}")]
    public async Task<IEnumerable<MessageElastic>> FindMessagesByQuery(string query) =>
        await _elasticSearchService.FindMessagesByQueryAsync(query);
    */
    [HttpGet("{year:int}-{month:int}-{day:int}")]
    public async Task<IEnumerable<MessageElastic>> FindMessagesByDay(
        int year, int month, int day)
    {
        return await _elasticSearchService.FindMessagesByDayAsync(year, month, day);
    }
    /*
    [HttpGet("{year:int}-{month:int}-{day:int}-{hour:int}")]
    public async Task<IEnumerable<MessageElastic>> FindMessagesByHour(
        int year, int month, int day, int hour)
    {
        return new MessageElastic[1];
    }
    */
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
    /*
    [HttpPost]
    public async Task<IActionResult> Post(MessageElastic newMessage)
    {
        await _elasticSearchService.CreateAsync(newMessage);

        return CreatedAtAction(nameof(Get), new { id = newMessage.BsonId }, newMessage);
    }
    
    [HttpPost("multiple")]
    public async Task<IActionResult> PostMany(List<MessageElastic> newMessages)
    {
        if (newMessages.Count == 0)
        {
            return BadRequest("No messages provided for insertion.");
        }

        await _elasticSearchService.CreateManyAsync(newMessages);

        return CreatedAtAction(nameof(Get), $"inserted {newMessages.Count} messages");
    }
    
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, MessageElastic updatedMessage)
    {
        var message = await _elasticSearchService.GetAsync(id);

        if (message is null)
        {
            return NotFound();
        }

        //updatedMessage.Id = message.Id;

        await _elasticSearchService.UpdateAsync(id, updatedMessage);

        return NoContent();
    }
    
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var message = await _elasticSearchService.GetAsync(id);
        
        if (message is null)
        {
            return NotFound();
        }

        await _elasticSearchService.DeleteAsync(id);

        return NoContent();
    }
    
    
    [HttpDelete("delete-all")]
    public async Task<IActionResult> DeleteAll()
    {
        await _elasticSearchService.DeleteAllAsync();

        return NoContent();
    }
    */
}