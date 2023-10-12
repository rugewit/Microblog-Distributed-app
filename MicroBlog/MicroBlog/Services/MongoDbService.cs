using MicroBlog.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MicroBlog.Services;

public class MongoDbService
{
    public IMongoDatabase MongoDatabase { get; }

    public MongoDbService(IOptions<MicroBlogDatabaseSettings> databaseSettings)
    {
        Console.WriteLine("MongoDbService CONSTRUCTOR");
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        MongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);
    }
}