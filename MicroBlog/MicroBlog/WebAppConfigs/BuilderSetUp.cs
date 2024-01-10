using Enyim.Caching.Configuration;
using MicroBlog.Models.Settings;
using MicroBlog.Providers;
using MicroBlog.Providers.Interfaces;
using MicroBlog.Services;
using MicroBlog.Services.Interfaces;

namespace MicroBlog.WebAppConfigs;

public static class BuilderSetUp
{
    public static void SetUp(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // datasets path configuration
        builder.Services.Configure<DatasetPathSettings>(
            builder.Configuration.GetSection("DatasetPath"));
        // expire policy
        builder.Services.Configure<UserAccountsExpirePolicySettings>(
            builder.Configuration.GetSection("UserAccountsExpirePolicy"));
        
        var mongoDbSection = "MicroBlogDatabase";
        var elasticSearchSection = "ElasticSearch";
        var memCacheSection = "MemCacheSettings";
        var redisSection = "RedisSettings";
        
        // for development
        if (builder.Environment.IsDevelopment())
        {
            // add for every postfix
            mongoDbSection += "Dev";
            elasticSearchSection += "Dev";
            memCacheSection += "Dev";
            redisSection += "Dev";
        }
        //Console.WriteLine($"mongoDbSection={mongoDbSection}");
        // mongo db configuration
        builder.Services.Configure<MicroBlogDatabaseSettings>(
            builder.Configuration.GetSection(mongoDbSection));
        // elastic search configuration
        builder.Services.Configure<ElasticSearchSettings>(
            builder.Configuration.GetSection(elasticSearchSection));
        // memcache configuration
        builder.Services.Configure<MemCacheSettings>(
            builder.Configuration.GetSection(memCacheSection));
        // redis configuration
        builder.Services.Configure<RedisSettings>(
            builder.Configuration.GetSection(redisSection));
        
        builder.Services.AddEnyimMemcached(o => o.Servers =
        [
            new Server { Address = "localhost", Port = 11211 },
            new Server { Address = "localhost", Port = 11212 },
            new Server { Address = "localhost", Port = 11213 }
        ]);
        
        builder.Services.AddSingleton<IMongoDbProvider, MongoDbProvider>();
        builder.Services.AddSingleton<IMemCacheProvider, MemCacheProvider>();
        builder.Services.AddSingleton<IRedisProvider, RedisProvider>();
        builder.Services.AddSingleton<IElasticSearchProvider, ElasticSearchProvider>();
        builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();
        builder.Services.AddSingleton<IUserAccountsService, UserAccountsService>();
        builder.Services.AddSingleton<IMessagesService, MessagesService>();
    }
}