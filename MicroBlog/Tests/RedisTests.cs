using MicroBlog.Models;
using MicroBlog.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public class RedisTests
{
    private WebApplication? _app;
    
    private WebApplication GetApp()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.Configure<MicroBlogDatabaseSettings>(
            builder.Configuration.GetSection("MicroBlogDatabase"));
        builder.Services.Configure<DatasetPathSettings>(
            builder.Configuration.GetSection("DatasetPath"));

        builder.Services.AddSingleton<MongoDbService>();
        builder.Services.AddSingleton<UserAccountsService>();
        builder.Services.AddSingleton<MessagesService>();
        builder.Services.AddSingleton(new RedisService("localhost"));
        
        var app = builder.Build();
        return app;
    }
    
    
    [Fact]
    public void SetAndGet()
    {
        _app = null ?? GetApp();
        var redisDb = _app.Services.GetRequiredService<IRedisService>().GetRedisDb();

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