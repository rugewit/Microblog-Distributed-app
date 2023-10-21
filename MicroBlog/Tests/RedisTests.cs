using MicroBlog.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public class RedisTests
{
    [Fact]
    public void SetAndGet()
    {
        var app = WebAppForTest.GetTestApp();
        var redisDb = app.Services.GetRequiredService<IRedisService>().GetRedisDb();

        var pair1 = new { key = "Ivan", value = "Petrov" };
        var pair2 = new { key = "Ivan1", value = "Petrov2" };
        
        redisDb.StringSet(pair1.key, pair1.value);
        redisDb.StringSet(pair2.key, pair2.value);
        
        Assert.True(redisDb.StringGet(pair1.key).HasValue);
        Assert.Equal(pair1.value, redisDb.StringGet(pair1.key).ToString());
        
        Assert.True(redisDb.StringGet(pair2.key).HasValue);
        Assert.Equal(pair2.value, redisDb.StringGet(pair2.key).ToString());
    }
}