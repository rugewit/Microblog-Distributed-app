using MicroBlog.Models;

namespace MicroBlog.Services;

public interface IUserAccountsService
{
    public int GetAllDocsLimit { get; set; }
    
    public Task<List<UserAccount>> GetAsync();

    public Task<List<UserAccount>> GetLimitedAsync(int limit);

    public Task<UserAccount?> GetAsync(string id);

    public Task CreateAsync(UserAccount newUserAccount);

    public Task CreateManyAsync(IEnumerable<UserAccount> newUserAccounts);

    public Task UpdateAsync(string id, UserAccount updatedUserAccount);

    public Task RemoveAsync(string id);
}