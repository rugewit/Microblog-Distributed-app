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
    private const string LockIdentifier = "hfgf234832645tfedf";
    
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
        UserAccount? userAccount;
        if (await _redisDb.KeyExistsAsync(id))
        {
            var userAccountRedisValue = await _redisDb.StringGetAsync(id);
            userAccount = JsonSerializer.Deserialize<UserAccount>(userAccountRedisValue.ToString());
            Console.WriteLine("got from redis");
        }
        else
        {
            userAccount = await _userAccountsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (userAccount != null)
            {
                var userAccountString = JsonSerializer.Serialize(userAccount);
                await _redisDb.StringSetAsync(id, userAccountString);
                Console.WriteLine("got from db");
            }
        }
        return userAccount;
    }
    
    public async Task CreateAsync(UserAccount newUserAccount) =>
        await _userAccountsCollection.InsertOneAsync(newUserAccount);

    public async Task CreateManyAsync(IEnumerable<UserAccount> newUserAccounts) =>
        await _userAccountsCollection.InsertManyAsync(newUserAccounts);

    public async Task UpdateAsync(string id, UserAccount updatedUserAccount)
    {
        // lock user Account if it is in the redis cache
        if (await _redisDb.KeyExistsAsync(id))
        {
            var lockId = id + "1";
            var longOperationTime = TimeSpan.FromMinutes(1);
            
            if (await _redisDb.LockTakeAsync(lockId, LockIdentifier,
                    longOperationTime + TimeSpan.FromMinutes(1)))
            {
                await Task.Delay(longOperationTime);
                await _userAccountsCollection.ReplaceOneAsync(x => x.Id == id, updatedUserAccount);
                await _redisDb.StringSetAsync(id, JsonSerializer.Serialize(updatedUserAccount));
                if (!await _redisDb.LockReleaseAsync(lockId, LockIdentifier))
                {
                    Console.WriteLine($"Cannot unlock the lock,\nUserAccount={id},\nLockId={lockId}");
                }
                Console.WriteLine($"LockId={lockId} has been unlocked");
            }
            else
            {
                Console.WriteLine($"Cannot lock\nUserAccount={id},\nLockId={lockId}");
            }
        }
        else
        { 
            // replace in the mongo db
            await _userAccountsCollection.ReplaceOneAsync(x => x.Id == id, updatedUserAccount);
            // set in redis
            await _redisDb.StringSetAsync(id, JsonSerializer.Serialize(updatedUserAccount));
        }
    }

    public async Task RemoveAsync(string id)
    {
        if (await _redisDb.KeyExistsAsync(id))
        {
            await _redisDb.StringGetDeleteAsync(id);
        }
        await _userAccountsCollection.DeleteOneAsync(x => x.Id == id);
        
    }
}