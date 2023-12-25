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
        builder.Services.AddSingleton<IRedisProvider>(new RedisProvider("localhost,allowAdmin=true"));
        builder.Services.AddSingleton<IElasticSearchProvider, ElasticSearchProvider>();
        builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();
    }
}