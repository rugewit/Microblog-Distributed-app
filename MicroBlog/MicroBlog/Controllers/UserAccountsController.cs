using MicroBlog.Models;
using MicroBlog.Services;
using Microsoft.AspNetCore.Mvc;

namespace MicroBlog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserAccountsController : ControllerBase
{
    private readonly UserAccountsService _userAccountsService;

    public UserAccountsController(UserAccountsService userAccountsService)
    {
        _userAccountsService = userAccountsService;
    }
    
    [HttpGet]
    public async Task<List<UserAccount>> Get() =>
        await _userAccountsService.GetAsync();
    
    [HttpGet("{limit}")]
    public async Task<List<UserAccount>> Get(int limit) =>
        await _userAccountsService.GetLimitedAsync(limit);

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<UserAccount>> Get(string id)
    {
        var userAccount = await _userAccountsService.GetAsync(id);

        if (userAccount is null)
        {
            return NotFound();
        }

        return userAccount;
    }

    [HttpPost]
    public async Task<IActionResult> Post(UserAccount newUserAccount)
    {
        await _userAccountsService.CreateAsync(newUserAccount);

        return CreatedAtAction(nameof(Get), new { id = newUserAccount.Id }, newUserAccount);
    }
    
    [HttpPost("multiple")]
    public async Task<IActionResult> PostMany(List<UserAccount> newUserAccounts)
    {
        if (newUserAccounts == null || newUserAccounts.Count == 0)
        {
            return BadRequest("No user accounts provided for insertion.");
        }

        await _userAccountsService.CreateManyAsync(newUserAccounts);

        return CreatedAtAction(nameof(Get), new
        {
            ids = newUserAccounts.Select(userAccount => userAccount.Id)
        }, newUserAccounts);
    }
    
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, UserAccount updatedUserAccount)
    {
        var userAccount = await _userAccountsService.GetAsync(id);

        if (userAccount is null)
        {
            return NotFound();
        }

        updatedUserAccount.Id = userAccount.Id;

        await _userAccountsService.UpdateAsync(id, updatedUserAccount);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userAccount = await _userAccountsService.GetAsync(id);
        
        if (userAccount is null)
        {
            return NotFound();
        }

        await _userAccountsService.RemoveAsync(id);

        return NoContent();
    }
    
}