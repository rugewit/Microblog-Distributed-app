using MicroBlog.Models.Settings;
using MicroBlog.Providers.Interfaces;
using MicroBlog.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MicroBlog.Providers;

public class MongoDbProvider : IMongoDbProvider
{
    private readonly IMongoDatabase _db;
    public MongoDbProvider(IOptions<MicroBlogDatabaseSettings> databaseSettings, 
        ILogger<MongoDbProvider> logger)
    {
        logger.LogInformation("MongoDbService CONSTRUCTOR is up");
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        _db = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);
    }

    public IMongoDatabase GetDb()
    {
        return _db;
    }
}