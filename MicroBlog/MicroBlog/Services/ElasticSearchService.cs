using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using MicroBlog.Models;

namespace MicroBlog.Services;

public class ElasticSearchService
{
    /*
      private readonly ElasticsearchClient _elasticsearchClient;
       private readonly string _index;
       private readonly ILogger<ElasticSearchService> _logger;
    public ElasticSearchService(IElasticSearchProvider elasticSearchProvider, 
        IOptions<ElasticSearchSettings> elasticSearchSettings, ILogger<ElasticSearchService> logger)
    {
        _elasticsearchClient = elasticSearchProvider.ElasticClient;
        _logger = logger;
        _index = "messages";
        _elasticsearchClient.Indices.Create(_index);
    }
    */
    
    private ElasticsearchClient _elasticsearchClient;
    private string _index;
    private ILogger _logger;
    
    public ElasticSearchService(ILogger<ElasticSearchService> logger)
    {
        var nodes = new[]
        {
            new Uri("http://elasticsearch_node_01:9200"),
            new Uri("http://elasticsearch_node_02:9200"),
            new Uri("http://elasticsearch_node_03:9200"),
        };

        var connectionPool = new StaticNodePool(nodes);

        var settings = new ElasticsearchClientSettings(connectionPool);

        var client = new ElasticsearchClient(settings);
        
        _index = "messages";
        var responseCreateIndex = client.Indices.Create(_index);
        _elasticsearchClient = client;
        _logger = logger;
        Console.WriteLine("ElasticService constructor");
        _logger.LogInformation("ElasticService constructor");
    }
    
    public async Task CreateAsync(MessageElastic messageElastic)
    {
        var response = await _elasticsearchClient.IndexAsync(messageElastic, _index);
        //_logger.LogInformation($"Create: {response.IsValidResponse}");
    }
    
    public async Task CreateManyAsync(IEnumerable<MessageElastic> messages)
    {
        var response = await _elasticsearchClient.IndexManyAsync(messages, _index);
        //_logger.LogInformation($"Create: {response.IsValidResponse}");
    }

    public async Task<bool> UpdateAsync(string id, MessageElastic newMessage)
    {
        var doc = await _elasticsearchClient.SearchAsync<MessageElastic>(s
            => s.Index(_index)
                .Query(q 
                    => q.Term(t => t.BsonId, id)
                )
        );
        var resUpdate = await _elasticsearchClient.UpdateAsync<MessageElastic, MessageElastic>(_index,
            doc.Hits.FirstOrDefault().Id, u => u.Doc(newMessage));
        if (!resUpdate.IsValidResponse)
        {
            return false;
        }
        return true;
    }

    public Task<IEnumerable<MessageElastic>> FindMessagesByQueryAsync(string incomeQuery)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(string id)
    {
        var doc = await _elasticsearchClient.SearchAsync<MessageElastic>(s
            => s.Index(_index)
                .Query(q 
                    => q.Term(t => t.BsonId, id)
                )
        );
        var deleteReq = new DeleteRequest(_index, doc.Hits.FirstOrDefault().Id);
        await _elasticsearchClient.DeleteAsync(deleteReq);
    }

    public async Task<MessageElastic> GetAsync(string id)
    {
        var response = await _elasticsearchClient.SearchAsync<MessageElastic>(s
            => s.Index(_index)
                .Query(q 
                    => q.Term(t => t.BsonId, id)
                )
            );
        //_logger.LogInformation($"Get: {response.IsValidResponse}");
        return response.Documents.FirstOrDefault();
    }
    
    public async Task<IEnumerable<MessageElastic>> GetAllAsync()
    {
        var response = await _elasticsearchClient.SearchAsync<MessageElastic>(s
            => s.Index(_index));
        //_logger.LogInformation($"GetAll: {response.IsValidResponse}");
        return response.Documents;
    }

    public async Task<IEnumerable<MessageElastic>> GetLimitedAsync(int limit = 200)
    {
        var response = await _elasticsearchClient.SearchAsync<MessageElastic>(s
            => s.Index(_index).From(0).Size(limit));
        return response.Documents;
    }

    public async Task<long> GetTotalCountAsync()
    {
        var countRequest = new CountRequest(Indices.Index(_index));
        var count = (await _elasticsearchClient.CountAsync(countRequest)).Count;
        return count;
    }

    public async Task<IEnumerable<MessageElastic>> FindMessagesByDayAsync(int year, int month, int day)
    {
        var targetDate = new DateTime(year, month, day);
        var searchResponse = await _elasticsearchClient.SearchAsync<MessageElastic>(s => s
            .Index(_index)
            .Query(q => q
                .Range(r => r
                    .DateRange(f => f.Field(
                        h => h.CreationDate)
                        .Gte(targetDate.Date).Lt(targetDate.Date.AddDays(1)))
                )
            )
            .Size(1000)
        );
        //_logger.LogInformation($"FindMessagesByDayAsync: {searchResponse.IsValidResponse}");
        Console.WriteLine(searchResponse.Documents.Count);
        return searchResponse.Documents;
    }

    public Task<IEnumerable<MessageElastic>> FindMessagesByHourAsync(int year, int month, int day, int hour)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAllAsync()
    {
        await _elasticsearchClient.DeleteByQueryAsync<MessageElastic>(_index, q =>
            q.Query(rq => rq.MatchAll()));
    }
}