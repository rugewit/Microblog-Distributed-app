using MicroBlog.Models.Settings;
using MicroBlog.Providers.Interfaces;
using MicroBlog.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MicroBlog.Providers;

public class MongoDbProvider : IMongoDbProvider
{
    public IMongoDatabase MongoDatabase { get; }

    public MongoDbProvider(IOptions<MicroBlogDatabaseSettings> databaseSettings, 
        ILogger<MongoDbProvider> logger)
    {
        //Console.WriteLine("MongoDbService CONSTRUCTOR");
        logger.LogInformation("MongoDbService CONSTRUCTOR is up");
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        MongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);
    }
}