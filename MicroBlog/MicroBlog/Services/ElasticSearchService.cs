using Elasticsearch.Net;
using MicroBlog.Models;
using MicroBlog.Models.Settings;
using MicroBlog.Providers.Interfaces;
using MicroBlog.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Nest;

namespace MicroBlog.Services;

public class ElasticSearchService : IElasticSearchService
{
    private readonly ElasticClient _elasticsearchClient;
    private readonly string _indexName;
    
    public ElasticSearchService(IElasticSearchProvider elasticSearchProvider, 
        IOptions<ElasticSearchSettings> elasticSearchSettings)
    {
        _elasticsearchClient = elasticSearchProvider.ElasticsearchClient;
        _indexName = elasticSearchSettings.Value.IndexName;
    }
    
    public async Task CreateAsync(MessageElastic newMessage)
    {
        var res = await _elasticsearchClient.IndexAsync(newMessage, idx => idx
            .Index(_indexName)
            .Refresh(Refresh.WaitFor)
        );
        
        //var res = await _elasticsearchClient.IndexDocumentAsync(newMessage);
        if (!res.IsValid)
        {
            throw new Exception($"cannot index new Message Id={newMessage.BsonId}");
        }
    }
    
    public async Task CreateManyAsync(IEnumerable<MessageElastic> newMessages)
    {
        var res = await _elasticsearchClient.IndexManyAsync(newMessages);
    }

    public async Task<bool> UpdateAsync(string id, MessageElastic newMessage)
    {
        var doc = await _elasticsearchClient.SearchAsync<MessageElastic>(s => s
            .Index(_indexName)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.BsonId)
                    .Query(id)
                )
            )
        );
        
        var resUpdate = await _elasticsearchClient.UpdateAsync<MessageElastic, MessageElastic>
            ( doc.Hits.FirstOrDefault().Id, u => u.Doc(newMessage));
                
        if (!resUpdate.IsValid)
        {
            throw new Exception($"3 cannot index new Messages");
        }

        return true;
    }
    
    public async Task<MessageElastic> GetAsync(string id)
    {
        var doc = await _elasticsearchClient.SearchAsync<MessageElastic>(s => s
            .Index(_indexName)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.BsonId)
                    .Query(id)
                )
            )
        );
        
        if (!doc.IsValid)
        {
            throw new Exception($"1 cannot find index to update");
        }
        
        return doc.Documents.FirstOrDefault();
    }

    public async Task<IEnumerable<MessageElastic>> GetAllAsync()
    {
        var res = await _elasticsearchClient.SearchAsync<MessageElastic>(s => s
            .Index(_indexName));
        
        if (!res.IsValid)
        {
            throw new Exception($"cannot get all");
        }
        
        return res.Documents;
    }
    
    public async Task<IEnumerable<MessageElastic>> GetLimitedAsync(int limit=200)
    {
        var res = await _elasticsearchClient.SearchAsync<MessageElastic>(s => s
            .Index(_indexName)
            .Size(limit));
        
        if (!res.IsValid)
        {
            throw new Exception($"cannot get all");
        }
        //Console.WriteLine(res.Documents.Count);
        return res.Documents;
    }
    
    public async Task<int> GetTotalCountAsync()
    {
        var res = await _elasticsearchClient.SearchAsync<MessageElastic>(s => s
            .Index(_indexName));

        if (res.IsValid) return res.Documents.Count;
        
        Console.WriteLine("result is not valid");
        return 0;
    }

    public async Task<IEnumerable<MessageElastic>> FindMessagesByQueryAsync(string incomeQuery)
    {
        var res = await _elasticsearchClient.SearchAsync<MessageElastic>(s => s
            .Index(_indexName)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Body)
                    .Query(incomeQuery)
                )
            )
        );
        
        if (!res.IsValid)
        {
            throw new Exception($"cannot find messages");
        }

        return res.Documents;
    }
    
    public async Task<IEnumerable<MessageElastic>> FindMessagesByDayAsync(int year, int month, int day)
    {
        var targetDate = new DateTime(year, month, day);
        var searchResponse = await _elasticsearchClient.SearchAsync<MessageElastic>(s => s
            .Index(_indexName)
            .Query(q => q
                .DateRange(r => r
                    .Field(f => f.CreationDate)
                    .GreaterThanOrEquals(targetDate.Date)
                    .LessThan(targetDate.Date.AddDays(1))
                )
            )
            .Size(1000)
        );
        Console.WriteLine(searchResponse.Documents.Count);
        return searchResponse.Documents;
    }
    
    public Task<IEnumerable<MessageElastic>> FindMessagesByHourAsync(int year, int month, int day, int hour)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(string id)
    {
        /*
        var res = await _elasticsearchClient.DeleteAsync(id);
        
        if (!res.IsValid)
        {
            throw new Exception($"cannot delete messages");
        }
        */
        throw new NotImplementedException();
    }

    public string GetRootNodeInfo()
    {
        return _elasticsearchClient.RootNodeInfo().ToString();
    }
}