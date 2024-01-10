using MicroBlog.Models;
using MicroBlog.Models.Settings;
using MicroBlog.Providers.Interfaces;
using MicroBlog.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MicroBlog.Services;

public class MessagesService : IMessagesService
{
    private readonly IMongoCollection<Message> _messagesCollection;
    
    public MessagesService(IMongoDbProvider mongoDbProvider, 
        IOptions<MicroBlogDatabaseSettings> databaseSettings)
    {
        var collectionName = databaseSettings.Value.MessagesCollectionName;
        _messagesCollection = mongoDbProvider.GetDb().GetCollection<Message>(collectionName);
    }

    public async Task<IEnumerable<Message>> GetAllAsync() =>
        await _messagesCollection.Find(_ => true).ToListAsync();
    
    public async Task<IEnumerable<Message>> GetLimitedAsync(int limit = 200) =>
        await _messagesCollection.Find(_ => true).Limit(limit).ToListAsync();

    public async Task<long> GetTotalCount()
    {
        var count = await _messagesCollection.CountDocumentsAsync(_ => true);
        return count;
    }

    public async Task<Message?> GetAsync(string id) =>
        await _messagesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Message newMessage)
    {
        await _messagesCollection.InsertOneAsync(newMessage);
    }
    
    public async Task CreateManyAsync(IEnumerable<Message> messages) =>
        await _messagesCollection.InsertManyAsync(messages);

    public async Task UpdateAsync(string id, Message updatedMessage) =>
        await _messagesCollection.ReplaceOneAsync(x => x.Id == id, updatedMessage);

    public async Task RemoveAsync(string id) =>
        await _messagesCollection.DeleteOneAsync(x => x.Id == id);

    public async Task RemoveAllAsync()
    {
        await _messagesCollection.DeleteManyAsync(_ => true);
    }
}