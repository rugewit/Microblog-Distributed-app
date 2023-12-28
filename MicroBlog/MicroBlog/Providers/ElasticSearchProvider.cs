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
    public ElasticsearchClient ElasticClient { get; }

    public ElasticSearchProvider(IOptions<ElasticSearchSettings> elasticSearchSettings)
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
        
        ElasticClient = client;
    }
}