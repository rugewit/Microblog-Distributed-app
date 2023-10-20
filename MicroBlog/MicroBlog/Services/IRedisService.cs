using StackExchange.Redis;

namespace MicroBlog.Services;

public interface IRedisService
{
    public IDatabase GetRedisDb();
}