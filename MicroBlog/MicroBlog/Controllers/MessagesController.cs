using MicroBlog.Models;
using MicroBlog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoUtils = MicroBlog.Utils.MongoUtils;

namespace MicroBlog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessagesService _messagesService;

    public MessagesController(IMessagesService messagesService)
    {
        _messagesService = messagesService;
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
    public async Task<long> GetTotalCount()
    {
        var totalCount = await _messagesService.GetTotalCount();
        
        return totalCount;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Message newMessage)
    {
        await _messagesService.CreateAsync(newMessage);

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

        return NoContent();
    }
    
}