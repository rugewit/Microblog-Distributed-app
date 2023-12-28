using MicroBlog.Models;
using MicroBlog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoUtils = MicroBlog.Utils.MongoUtils;

namespace MicroBlog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserAccountsController : ControllerBase
{
    private readonly IUserAccountsService _userAccountsService;

    public UserAccountsController(IUserAccountsService userAccountsService)
    {
        _userAccountsService = userAccountsService;
    }
    
    [HttpGet]
    public async Task<IEnumerable<UserAccount>> Get() =>
        await _userAccountsService.GetLimitedAsync();
    
    [HttpGet("{limit:int}")]
    public async Task<IEnumerable<UserAccount>> Get(int limit) =>
        await _userAccountsService.GetLimitedAsync(limit);

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<UserAccount>> Get(string id)
    {
        if (!MongoUtils.IsValidMongoId(id))
        {
            return BadRequest("invalid mongo id has been provided");
        }
        
        var userAccount = await _userAccountsService.GetAsync(id);
        
        if (userAccount is null)
        {
            return NotFound();
        }
        return userAccount;
    }

    [HttpGet("total-count")]
    public async Task<long> GetTotalCount()
    {
        var count = await _userAccountsService.GetTotalCount();
        return count;
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
        if (newUserAccounts.Count == 0)
        {
            return BadRequest("No user accounts provided for insertion.");
        }

        await _userAccountsService.CreateManyAsync(newUserAccounts);

        return CreatedAtAction(nameof(Get), $"inserted: {newUserAccounts.Count} users");
    }
    
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, UserAccount updatedUserAccount)
    {
        if (!MongoUtils.IsValidMongoId(id))
        {
            return BadRequest("invalid mongo id has been provided");
        }
        var userAccount = await _userAccountsService.GetAsync(id);

        if (userAccount is null)
        {
            return NotFound();
        }

        updatedUserAccount.Id = userAccount.Id;

        if (await _userAccountsService.UpdateAsync(id, updatedUserAccount))
        {
            return NoContent();
        }
        // it means that id is already locked
        const int lockStatusCode = 423;
        return StatusCode(lockStatusCode);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!MongoUtils.IsValidMongoId(id))
        {
            return BadRequest("invalid mongo id has been provided");
        }
        var userAccount = await _userAccountsService.GetAsync(id);
        
        if (userAccount is null)
        {
            return NotFound();
        }

        await _userAccountsService.RemoveAsync(id);

        return NoContent();
    }
    
    [HttpDelete("delete-all")]
    public async Task<IActionResult> DeleteAll()
    {
        await _userAccountsService.RemoveAllAsync();

        return NoContent();
    }
}