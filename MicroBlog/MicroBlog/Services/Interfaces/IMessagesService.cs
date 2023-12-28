using MicroBlog.Models;

namespace MicroBlog.Services.Interfaces;

public interface IMessagesService
{
    public Task<IEnumerable<Message>> GetAllAsync();

    public Task<IEnumerable<Message>> GetLimitedAsync(int limit = 200);

    public Task<long> GetTotalCount();

    public Task<Message?> GetAsync(string id);

    public Task CreateAsync(Message newMessage);

    public Task CreateManyAsync(IEnumerable<Message> messages);
    
    public Task UpdateAsync(string id, Message updatedMessage);

    public Task RemoveAsync(string id);
}