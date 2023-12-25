using StackExchange.Redis;

namespace MicroBlog.Providers.Interfaces;

public interface IRedisProvider
{
    public IDatabase GetRedisDb();
}