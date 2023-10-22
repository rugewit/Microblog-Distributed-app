using StackExchange.Redis;

namespace MicroBlog.Services;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;
    
    public RedisService(string address)
    {
        Console.WriteLine("RedisService CONSTRUCTOR");
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
        Console.WriteLine($"Endpoints length is {endpoints.Length}");
        foreach (var endpoint in endpoints)
        {
            var server = redis.GetServer(endpoint);
            server.FlushAllDatabases();
        }
    }
}