using StackExchange.Redis;

namespace MicroBlog.Services.Interfaces;

public interface IRedisService
{
    public IDatabase GetRedisDb();
}