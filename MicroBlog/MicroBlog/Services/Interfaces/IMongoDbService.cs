using MongoDB.Driver;

namespace MicroBlog.Services.Interfaces;

public interface IMongoDbService
{
    public IMongoDatabase MongoDatabase { get; }
}