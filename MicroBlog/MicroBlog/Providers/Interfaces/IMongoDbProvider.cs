using MongoDB.Driver;

namespace MicroBlog.Providers.Interfaces;

public interface IMongoDbProvider
{
    public IMongoDatabase MongoDatabase { get; }
}