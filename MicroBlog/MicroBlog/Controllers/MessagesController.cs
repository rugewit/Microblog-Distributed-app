using System.Runtime.InteropServices.ComTypes;
using MicroBlog.Models;
using MicroBlog.Services;
using MicroBlog.Services.Interfaces;
using MicroBlog.Utils;
using Microsoft.AspNetCore.Mvc;
using MongoUtils = MicroBlog.Utils.MongoUtils;

namespace MicroBlog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessagesService _messagesService;
    private readonly ElasticSearchService _elasticSearchService;

    public MessagesController(IMessagesService messagesService, ElasticSearchService elasticSearchService)
    {
        _messagesService = messagesService;
        _elasticSearchService = elasticSearchService;
    }
    
    [HttpGet]
    public async Task<IEnumerable<Message>> Get() =>
        await _messagesService.GetLimitedAsync();
    
    [HttpGet("{limit}")]
    public async Task<IEnumerable<Message>> Get(int limit) =>
        await _messagesService.GetLimitedAsync(limit);
    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Message>> Get(string id)
    {
        if (!MongoUtils.IsValidMongoId(id))
        {
            return BadRequest("invalid mongo id has been provided");
        }
        
        var message = await _messagesService.GetAsync(id);

        if (message is null)
        {
            return NotFound();
        }

        return message;
    }
    
    [HttpGet("total-count")]
    public async Task<IActionResult> GetTotalCount()
    {
        var totalCountDb = await _messagesService.GetTotalCount();
        var totalCountElastic = await _elasticSearchService.GetTotalCountAsync();
        
        return Ok($"mongo messages: {totalCountDb}, elastic messages: {totalCountElastic}");
    }

    [HttpPost]
    public async Task<IActionResult> Post(Message newMessage)
    {
        await _messagesService.CreateAsync(newMessage);

        var elasticMessage = ModelsUtils.MessageToMessageElastic(newMessage);

        await _elasticSearchService.CreateAsync(elasticMessage);

        return CreatedAtAction(nameof(Get), new { id = newMessage.Id }, newMessage);
    }
    
    [HttpPost("multiple")]
    public async Task<IActionResult> PostMany(List<Message> newMessages)
    {
        if (newMessages.Count == 0)
        {
            return BadRequest("No messages provided for insertion.");
        }

        await _messagesService.CreateManyAsync(newMessages);

        var elasticMessages = new List<MessageElastic>(newMessages.Count);
        elasticMessages.AddRange(newMessages.Select(ModelsUtils.MessageToMessageElastic));

        await _elasticSearchService.CreateManyAsync(elasticMessages);
        
        return CreatedAtAction(nameof(Get), $"inserted: {newMessages.Count} messages");
    }
    
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Message updatedMessage)
    {
        if (!MongoUtils.IsValidMongoId(id))
        {
            return BadRequest("invalid mongo id has been provided");
        }
        var message = await _messagesService.GetAsync(id);

        if (message is null)
        {
            return NotFound();
        }

        updatedMessage.Id = message.Id;

        await _messagesService.UpdateAsync(id, updatedMessage);

        var elasticMessage = ModelsUtils.MessageToMessageElastic(updatedMessage);
        await _elasticSearchService.UpdateAsync(id, elasticMessage);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!MongoUtils.IsValidMongoId(id))
        {
            return BadRequest("invalid mongo id has been provided");
        }
        var message = await _messagesService.GetAsync(id);
        
        if (message is null)
        {
            return NotFound();
        }

        await _messagesService.RemoveAsync(id);
        await _elasticSearchService.DeleteAsync(id);

        return NoContent();
    }
    
    [HttpDelete("delete-all")]
    public async Task<IActionResult> DeleteAll()
    {
        await _messagesService.RemoveAllAsync();
        await _elasticSearchService.DeleteAllAsync();
        
        return NoContent();
    }
}