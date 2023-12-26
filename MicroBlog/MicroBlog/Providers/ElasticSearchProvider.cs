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
        var address = new Uri(elasticSearchSettings.Value.ConnectionString);
        //var auth = new BasicAu(elasticSearchSettings.Value.Username,
        //    elasticSearchSettings.Value.Password);
        var settings = new ConnectionSettings(address)
            .CertificateFingerprint(elasticSearchSettings.Value.CertificateFingerprint)
            .BasicAuthentication(elasticSearchSettings.Value.Username, elasticSearchSettings.Value.Password)
            .DefaultIndex(elasticSearchSettings.Value.IndexName);
        ElasticsearchClient = new ElasticClient(settings);
    }
}