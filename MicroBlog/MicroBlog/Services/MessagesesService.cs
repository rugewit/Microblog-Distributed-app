using MicroBlog.Models;
using MicroBlog.Models.Settings;
using MicroBlog.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MicroBlog.Services;

public class MessagesService : IMessagesService
{
    public int GetAllDocsLimit { get; set; } = 200;
    private readonly IMongoCollection<Message> _messagesCollection;
    
    public MessagesService(IMongoDbService mongoDbService, 
        IOptions<MicroBlogDatabaseSettings> databaseSettings)
    {
        var collectionName = databaseSettings.Value.MessagesCollectionName;
        _messagesCollection = mongoDbService.MongoDatabase.GetCollection<Message>(collectionName);
    }
    
    public async Task<List<Message>> GetAsync() =>
        await _messagesCollection.Find(_ => true).Limit(GetAllDocsLimit).ToListAsync();
    
    public async Task<List<Message>> GetLimitedAsync(int limit) =>
        await _messagesCollection.Find(_ => true).Limit(limit).ToListAsync();

    public async Task<Message?> GetAsync(string id) =>
        await _messagesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Message newMessage) =>
        await _messagesCollection.InsertOneAsync(newMessage);

    public async Task CreateManyAsync(IEnumerable<Message> messages) =>
        await _messagesCollection.InsertManyAsync(messages);

    public async Task UpdateAsync(string id, Message updatedMessage) =>
        await _messagesCollection.ReplaceOneAsync(x => x.Id == id, updatedMessage);

    public async Task RemoveAsync(string id) =>
        await _messagesCollection.DeleteOneAsync(x => x.Id == id);
}