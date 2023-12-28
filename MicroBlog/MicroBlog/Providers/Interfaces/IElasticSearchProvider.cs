//using Elastic.Clients.Elasticsearch;

using Elastic.Clients.Elasticsearch;

namespace MicroBlog.Providers.Interfaces;

public interface IElasticSearchProvider
{
    public ElasticsearchClient ElasticClient { get; }
}