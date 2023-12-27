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
        // mongo db configuration
        builder.Services.Configure<MicroBlogDatabaseSettings>(
            builder.Configuration.GetSection("MicroBlogDatabase"));
        // datasets path configuration
        builder.Services.Configure<DatasetPathSettings>(
            builder.Configuration.GetSection("DatasetPath"));
        // expire policy
        builder.Services.Configure<UserAccountsExpirePolicySettings>(
            builder.Configuration.GetSection("UserAccountsExpirePolicy"));
        // elastic search configuration
        builder.Services.Configure<ElasticSearchSettings>(
            builder.Configuration.GetSection("ElasticSearch"));
        
        builder.Services.AddSingleton<IMongoDbProvider, MongoDbProvider>();
        builder.Services.AddSingleton<IUserAccountsService, UserAccountsService>();
        builder.Services.AddSingleton<IMessagesService, MessagesService>();
        builder.Services.AddSingleton<IRedisProvider>(new RedisProvider("redis:6379,allowAdmin=true"));
        builder.Services.AddSingleton<IElasticSearchProvider, ElasticSearchProvider>();
        builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();
        builder.Services.AddEnyimMemcached(o => o.Servers = new List<Server> { 
            new Server { Address = "memcached_node_01", Port = 11211 },
            new Server { Address = "memcached_node_02", Port = 11211 },
            new Server { Address = "memcached_node_03", Port = 11211 },
             });
        builder.Services.AddSingleton<IMemCacheProvider, MemCacheProvider>();
    }
}