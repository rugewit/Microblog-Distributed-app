using MicroBlog.Models;

namespace MicroBlog.Services.Interfaces;

public interface IUserAccountsService
{
    public Task<IEnumerable<UserAccount>> GetAllAsync();
    
    public Task<IEnumerable<UserAccount>> GetLimitedAsync(int limit = 200);

    public Task<UserAccount?> GetAsync(string id);
    
    public Task<long> GetTotalCount();
    
    public Task CreateAsync(UserAccount newUserAccount);

    public Task CreateManyAsync(IEnumerable<UserAccount> newUserAccounts);

    public Task<bool> UpdateAsync(string id, UserAccount updatedUserAccount);

    public Task RemoveAsync(string id);
}