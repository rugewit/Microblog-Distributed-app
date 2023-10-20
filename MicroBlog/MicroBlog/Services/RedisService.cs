using StackExchange.Redis;

namespace MicroBlog.Services;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;
    
    public RedisService(string address)
    {
        var redis = ConnectionMultiplexer.Connect(address);
        _db = redis.GetDatabase();
    }

    public IDatabase GetRedisDb()
    {
        return _db;
    }
}