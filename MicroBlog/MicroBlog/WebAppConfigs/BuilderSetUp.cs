using MicroBlog.Models;
using MicroBlog.Services;

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

        builder.Services.AddSingleton<IMongoDbService, MongoDbService>();
        builder.Services.AddSingleton<IUserAccountsService, UserAccountsService>();
        builder.Services.AddSingleton<IMessagesService, MessagesService>();
        builder.Services.AddSingleton<IRedisService>(new RedisService("localhost"));
    }
}