using MicroBlog.Models;


namespace MicroBlog.Services;

public interface IMessagesService
{
    public int GetAllDocsLimit { get; set; }
    
    public Task<List<Message>> GetAsync();

    public Task<List<Message>> GetLimitedAsync(int limit);

    public Task<Message?> GetAsync(string id);

    public Task CreateAsync(Message newMessage);

    public Task CreateManyAsync(IEnumerable<Message> messages);
    
    public Task UpdateAsync(string id, Message updatedMessage);

    public Task RemoveAsync(string id);
}