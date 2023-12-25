//using Elastic.Clients.Elasticsearch;
using Nest;

namespace MicroBlog.Providers.Interfaces;

public interface IElasticSearchProvider
{
    public ElasticClient ElasticsearchClient { get; }
}