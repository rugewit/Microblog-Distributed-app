using MicroBlog.Models;
using MicroBlog.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<List<Message>> Get() =>
        await _messagesService.GetAsync();
    
    [HttpGet("{limit}")]
    public async Task<List<Message>> Get(int limit) =>
        await _messagesService.GetLimitedAsync(limit);
    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Message>> Get(string id)
    {
        var message = await _messagesService.GetAsync(id);

        if (message is null)
        {
            return NotFound();
        }

        return message;
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

        return CreatedAtAction(nameof(Get), new
        {
            ids = newMessages.Select(message => message.Id)
        }, newMessages);
    }
    
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Message updatedMessage)
    {
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
        var message = await _messagesService.GetAsync(id);
        
        if (message is null)
        {
            return NotFound();
        }

        await _messagesService.RemoveAsync(id);

        return NoContent();
    }
    
}