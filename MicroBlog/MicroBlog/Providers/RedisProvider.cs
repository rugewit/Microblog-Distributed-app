using MicroBlog.Providers.Interfaces;
using StackExchange.Redis;

namespace MicroBlog.Providers;

public class RedisProvider : IRedisProvider
{
    private readonly IDatabase _db;
    
    public RedisProvider(string address)
    {
        //Console.WriteLine("RedisService CONSTRUCTOR");
        //logger.LogInformation("RedisService CONSTRUCTOR");
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
        //Console.WriteLine($"Endpoints length is {endpoints.Length}");
        foreach (var endpoint in endpoints)
        {
            var server = redis.GetServer(endpoint);
            server.FlushAllDatabases();
        }
    }
}