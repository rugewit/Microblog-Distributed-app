using System.Text.Json;
using Enyim.Caching;
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
    private readonly IMongoCollection<UserAccount> _userAccountsCollection;
    private readonly IDatabase _redisDb;
    private readonly int _userAccExpireTimeSec;
    private readonly int _redactionTimeSimulationSec;
    // some random identifier for the redis lock
    private const string LockIdentifier = "hfgf234832645tfedf";
    private readonly ILogger<UserAccountsService> _logger;
    private readonly IMemcachedClient _memCache;
    
    public UserAccountsService(IMongoDbProvider mongoDbProvider,
        IOptions<MicroBlogDatabaseSettings> databaseSettings, IRedisProvider redisProvider,
        IOptions<UserAccountsExpirePolicySettings> userAccExpireSettings,
        ILogger<UserAccountsService> logger, IMemCacheProvider memCacheProvider)
    {
        var collectionName = databaseSettings.Value.UserAccountsCollectionName;
        _userAccountsCollection = mongoDbProvider.GetDb().GetCollection<UserAccount>(collectionName);
        _redisDb = redisProvider.GetRedisDb();
        _logger = logger;

        _userAccExpireTimeSec = userAccExpireSettings.Value.UserAccountCacheExpireTimeSec;
        _redactionTimeSimulationSec = userAccExpireSettings.Value.RedactionTimeSimulationSec;
        _memCache = memCacheProvider.GetClient();
    }
    
    public async Task<IEnumerable<UserAccount>> GetLimitedAsync(int limit = 200) =>
        await _userAccountsCollection.Find(_ => true).Limit(limit).ToListAsync();

    public async Task<IEnumerable<UserAccount>> GetAllAsync() => 
        await _userAccountsCollection.Find(_ => true).ToListAsync();
    
    public async Task<long> GetTotalCount() => 
        await _userAccountsCollection.CountDocumentsAsync(_ => true);
    
    public async Task<UserAccount?> GetAsync(string id)
    {
        UserAccount? userAccount;
        // REPLACE REDIS TO MEMCACHED
        // await _redisDb.KeyExistsAsync(id)
        var memCachedRes = await _memCache.GetAsync<string>(id);
        if (memCachedRes.Success)
        {
            // REPLACE REDIS TO MEMCACHED
            //var userAccountRedisValue = await _redisDb.StringGetAsync(id);
            //var userAccountMemCachedValue = await _memCache.GetAsync<string>(id);
            userAccount = JsonSerializer.Deserialize<UserAccount>(memCachedRes.Value);
            //Console.WriteLine("got from redis");
            _logger.LogInformation($"{id} got from memcached");
        }
        else
        {
            userAccount = await _userAccountsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (userAccount != null)
            {
                var userAccountString = JsonSerializer.Serialize(userAccount);
                // REPLACE REDIS TO MEMCACHED
                //await _redisDb.StringSetAsync(id, userAccountString, TimeSpan.FromSeconds(_userAccExpireTimeSec));
                await _memCache.AddAsync(id, userAccountString, TimeSpan.FromSeconds(_userAccExpireTimeSec));
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
        var lockId = id + "1";
        var redactionSimulationTime = TimeSpan.FromSeconds(_redactionTimeSimulationSec);
        
        if (await _redisDb.LockTakeAsync(lockId, LockIdentifier,
                // lock expiry must be greater then redactionSimulationTime
                redactionSimulationTime * 2))
        {
            await Task.Delay(redactionSimulationTime);
            await _userAccountsCollection.ReplaceOneAsync(x => x.Id == id, updatedUserAccount);
            // REPLACE REDIS TO MEMCACHED
            //await _redisDb.KeyDeleteAsync(id);
            await _memCache.RemoveAsync(id);
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
        // REPLACE REDIS TO MEMCACHED
        /*
        if (await _redisDb.KeyExistsAsync(id))
        {
            await _redisDb.StringGetDeleteAsync(id);
        }
        */
        var res = await _memCache.GetAsync<string>(id);
        if (res.Success)
        {
            await _memCache.RemoveAsync(id);
        }
        await _userAccountsCollection.DeleteOneAsync(x => x.Id == id);
    }
    
    public async Task RemoveAllAsync()
    {
        await _userAccountsCollection.DeleteManyAsync(_ => true);
    }
}