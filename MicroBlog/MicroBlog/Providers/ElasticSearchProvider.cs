//using Elastic.Clients.Elasticsearch;
//using Elastic.Transport;

using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using MicroBlog.Models.Settings;
using MicroBlog.Providers.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;


namespace MicroBlog.Providers;

public class ElasticSearchProvider: IElasticSearchProvider
{
    private readonly ElasticsearchClient _client;

    public ElasticSearchProvider(IOptions<ElasticSearchSettings> elasticSearchSettings)
    {
        var nodes = elasticSearchSettings.Value.ConnectionString.Select(x => new Uri(x)).ToList();
        var connectionPool = new StaticNodePool(nodes);

        var settings = new ElasticsearchClientSettings(connectionPool);

        var client = new ElasticsearchClient(settings);
        
        _client = client;
    }

    public ElasticsearchClient GetClient()
    {
        return _client;
    }
}