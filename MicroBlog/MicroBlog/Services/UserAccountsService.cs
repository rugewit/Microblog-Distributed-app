using System.Text.Json;
using MicroBlog.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StackExchange.Redis;

namespace MicroBlog.Services;

public class UserAccountsService : IUserAccountsService
{
    public int GetAllDocsLimit { get; set; } = 200;
    private readonly IMongoCollection<UserAccount> _userAccountsCollection;
    private readonly IDatabase _redisDb;
    
    public UserAccountsService(IMongoDbService mongoDbService, 
        IOptions<MicroBlogDatabaseSettings> databaseSettings, IRedisService redisService)
    {
        var collectionName = databaseSettings.Value.UserAccountsCollectionName;
        _userAccountsCollection = mongoDbService.MongoDatabase.GetCollection<UserAccount>(collectionName);
        _redisDb = redisService.GetRedisDb();
    }
    
    public async Task<List<UserAccount>> GetAsync() =>
        await _userAccountsCollection.Find(_ => true).Limit(GetAllDocsLimit).ToListAsync();
    
    public async Task<List<UserAccount>> GetLimitedAsync(int limit) =>
        await _userAccountsCollection.Find(_ => true).Limit(limit).ToListAsync();

    public async Task<UserAccount?> GetAsync(string id)
    {
        UserAccount? userAccount = null;
        // try to get userAccount from the redis cache
        var userAccountRedisValue = await _redisDb.StringGetAsync(id);
        if (!userAccountRedisValue.IsNull)
        {
            userAccount = JsonSerializer.Deserialize<UserAccount>(userAccountRedisValue.ToString());
        }
        if (userAccount is not null) return userAccount;
        // if an user account isn't in the redis cache
        userAccount = await _userAccountsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        // if is found from the db
        if (userAccount != null)
        {
            var userAccountString = JsonSerializer.Serialize(userAccount);
            await _redisDb.StringSetAsync(id, userAccountString, TimeSpan.FromMinutes(2));
        }

        return userAccount;
    }
    
    public async Task CreateAsync(UserAccount newUserAccount) =>
        await _userAccountsCollection.InsertOneAsync(newUserAccount);

    public async Task CreateManyAsync(IEnumerable<UserAccount> newUserAccounts) =>
        await _userAccountsCollection.InsertManyAsync(newUserAccounts);

    public async Task UpdateAsync(string id, UserAccount updatedUserAccount) =>
        await _userAccountsCollection.ReplaceOneAsync(x => x.Id == id, updatedUserAccount);

    public async Task RemoveAsync(string id) =>
        await _userAccountsCollection.DeleteOneAsync(x => x.Id == id);
}