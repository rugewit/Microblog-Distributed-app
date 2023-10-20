using MongoDB.Driver;

namespace MicroBlog.Services;

public interface IMongoDbService
{
    public IMongoDatabase MongoDatabase { get; }
}