using MicroBlog.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MicroBlog.Services;

public class MessagesService
{
    private readonly IMongoCollection<Message> _messagesCollection;
    
    public MessagesService(MongoDbService mongoDbService, 
        IOptions<MicroBlogDatabaseSettings> databaseSettings)
    {
        var collectionName = databaseSettings.Value.MessagesCollectionName;
        _messagesCollection = mongoDbService.MongoDatabase.GetCollection<Message>(collectionName);
    }
    
    public async Task<List<Message>> GetAsync() =>
        await _messagesCollection.Find(_ => true).ToListAsync();

    public async Task<Message?> GetAsync(string id) =>
        await _messagesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Message newMessage) =>
        await _messagesCollection.InsertOneAsync(newMessage);

    public async Task CreateManyAsync(List<Message> messages) =>
        await _messagesCollection.InsertManyAsync(messages);

    public async Task UpdateAsync(string id, Message updatedMessage) =>
        await _messagesCollection.ReplaceOneAsync(x => x.Id == id, updatedMessage);

    public async Task RemoveAsync(string id) =>
        await _messagesCollection.DeleteOneAsync(x => x.Id == id);
}