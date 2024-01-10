using MicroBlog.Models.Settings;
using MicroBlog.Providers.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MicroBlog.Providers;

public class RedisProvider : IRedisProvider
{
    private readonly IDatabase _db;
    
    public RedisProvider(IOptions<RedisSettings> redisSettings)
    {
        var address = redisSettings.Value.ConnectionString;
        var redis = ConnectionMultiplexer.Connect(address);
        FlushAll(redis);
        _db = redis.GetDatabase();
    }

    public IDatabase GetRedisDb()
    {
        return _db;
    }

    private static void FlushAll(IConnectionMultiplexer redis)
    {
        var endpoints = redis.GetEndPoints();
        foreach (var endpoint in endpoints)
        {
            var server = redis.GetServer(endpoint);
            server.FlushAllDatabases();
        }
    }
}