using System.Text.Json;
using MicroBlog.Models;
using MicroBlog.Models.Settings;
using MicroBlog.Providers.Interfaces;
using MicroBlog.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StackExchange.Redis;

namespace MicroBlog.Services;

public class UserAccountsService : IUserAccountsService
{
    public int GetAllDocsLimit { get; set; } = 200;
    private readonly IMongoCollection<UserAccount> _userAccountsCollection;
    private readonly IDatabase _redisDb;
    private readonly int _userAccExpireTimeSec;
    private readonly int _redactionTimeSimulationSec;
    // some random identifier for the redis lock
    private const string LockIdentifier = "hfgf234832645tfedf";
    private readonly ILogger<UserAccountsService> _logger;
    
    public UserAccountsService(IMongoDbProvider mongoDbProvider,
        IOptions<MicroBlogDatabaseSettings> databaseSettings, IRedisProvider redisProvider,
        IOptions<UserAccountsExpirePolicySettings> userAccExpireSettings,
        ILogger<UserAccountsService> logger)
    {
        var collectionName = databaseSettings.Value.UserAccountsCollectionName;
        _userAccountsCollection = mongoDbProvider.MongoDatabase.GetCollection<UserAccount>(collectionName);
        _redisDb = redisProvider.GetRedisDb();
        _logger = logger;

        _userAccExpireTimeSec = userAccExpireSettings.Value.UserAccountCacheExpireTimeSec;
        _redactionTimeSimulationSec = userAccExpireSettings.Value.RedactionTimeSimulationSec;
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
            //Console.WriteLine("got from redis");
            _logger.LogInformation($"{id} got from redis");
        }
        else
        {
            userAccount = await _userAccountsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (userAccount != null)
            {
                var userAccountString = JsonSerializer.Serialize(userAccount);
                await _redisDb.StringSetAsync(id, userAccountString, 
                    TimeSpan.FromSeconds(_userAccExpireTimeSec));
                //Console.WriteLine("got from db");
                _logger.LogInformation($"{id} got from db");
            }
        }
        return userAccount;
    }
    
    public async Task CreateAsync(UserAccount newUserAccount) =>
        await _userAccountsCollection.InsertOneAsync(newUserAccount);

    public async Task CreateManyAsync(IEnumerable<UserAccount> newUserAccounts) =>
        await _userAccountsCollection.InsertManyAsync(newUserAccounts);

    public async Task<bool> UpdateAsync(string id, UserAccount updatedUserAccount)
    {
        // redis saves lock, account is already saved in redis, so weed to generate a new id
        var lockId = id + "1";
        var redactionSimulationTime = TimeSpan.FromSeconds(_redactionTimeSimulationSec);
        
        if (await _redisDb.LockTakeAsync(lockId, LockIdentifier,
                // lock expiry must be greater then redactionSimulationTime
                redactionSimulationTime * 2))
        {
            await Task.Delay(redactionSimulationTime);
            await _userAccountsCollection.ReplaceOneAsync(x => x.Id == id, updatedUserAccount);
            await _redisDb.KeyDeleteAsync(id);
            if (!await _redisDb.LockReleaseAsync(lockId, LockIdentifier))
            {
                _logger.LogCritical($"Cannot unlock the lock,\nUserAccount={id},\nLockId={lockId}");
            }
            _logger.LogInformation($"LockId={lockId} has been unlocked");
            return true;
        }
        _logger.LogInformation($"Cannot lock\nUserAccount={id},\nLockId={lockId}");
        return false;
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