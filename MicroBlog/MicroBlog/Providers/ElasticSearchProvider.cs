//using Elastic.Clients.Elasticsearch;
//using Elastic.Transport;

using Elasticsearch.Net;
using MicroBlog.Models.Settings;
using MicroBlog.Providers.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Nest;

namespace MicroBlog.Providers;

public class ElasticSearchProvider: IElasticSearchProvider
{
    public ElasticClient ElasticsearchClient { get; }

    public ElasticSearchProvider(IOptions<ElasticSearchSettings> elasticSearchSettings)
    {
        //var address = new Uri(elasticSearchSettings.Value.ConnectionString);
        // http://elasticsearch_node_01:9200,http://elasticsearch_node_02:9200,http://elasticsearch_node_03:9200
        var nodes = new[]
        {
            new Uri("http://elasticsearch_node_01:9200"),
            new Uri("http://elasticsearch_node_02:9200"),
            new Uri("http://elasticsearch_node_03:9200"),
        };

        var connectionPool = new SniffingConnectionPool(nodes);
        
        var settings = new ConnectionSettings(connectionPool)
            //.CertificateFingerprint(elasticSearchSettings.Value.CertificateFingerprint)
            //.BasicAuthentication(elasticSearchSettings.Value.Username, elasticSearchSettings.Value.Password)
            .DefaultIndex(elasticSearchSettings.Value.IndexName);
        ElasticsearchClient = new ElasticClient(settings);
    }
}