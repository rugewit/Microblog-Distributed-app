using MicroBlog.Models;

namespace MicroBlog.Services.Interfaces;

public interface IElasticSearchService
{
    public Task CreateAsync(MessageElastic newMessage);
    
    public Task CreateManyAsync(IEnumerable<MessageElastic> newMessages);
    
    public Task<bool> UpdateAsync(string id, MessageElastic newMessage);

    public Task<IEnumerable<MessageElastic>> FindMessagesByQueryAsync(string incomeQuery);

    public Task<IEnumerable<MessageElastic>> FindMessagesByDayAsync(int year, int month, int day);

    public Task<IEnumerable<MessageElastic>> FindMessagesByHourAsync(int year, int month, int day, int hour);

    public Task DeleteAsync(string id);
    
    public Task<MessageElastic> GetAsync(string id);
    
    public Task<IEnumerable<MessageElastic>> GetAllAsync();

    public Task<IEnumerable<MessageElastic>> GetLimitedAsync(int limit=200);

    public Task<long> GetTotalCountAsync();

    public Task DeleteAllAsync();
}