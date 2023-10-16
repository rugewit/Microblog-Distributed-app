using MicroBlog.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MicroBlog.Services;

public class UserAccountsService
{
    private readonly IMongoCollection<UserAccount> _userAccountsCollection;
    
    public UserAccountsService(MongoDbService mongoDbService, 
        IOptions<MicroBlogDatabaseSettings> databaseSettings)
    {
        var collectionName = databaseSettings.Value.UserAccountsCollectionName;
        _userAccountsCollection = mongoDbService.MongoDatabase.GetCollection<UserAccount>(collectionName);
    }
    
    public async Task<List<UserAccount>> GetAsync() =>
        await _userAccountsCollection.Find(_ => true).ToListAsync();

    public async Task<UserAccount?> GetAsync(string id) =>
        await _userAccountsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(UserAccount newUserAccount) =>
        await _userAccountsCollection.InsertOneAsync(newUserAccount);

    public async Task CreateManyAsync(IEnumerable<UserAccount> newUserAccounts) =>
        await _userAccountsCollection.InsertManyAsync(newUserAccounts);

    public async Task UpdateAsync(string id, UserAccount updatedUserAccount) =>
        await _userAccountsCollection.ReplaceOneAsync(x => x.Id == id, updatedUserAccount);

    public async Task RemoveAsync(string id) =>
        await _userAccountsCollection.DeleteOneAsync(x => x.Id == id);
}