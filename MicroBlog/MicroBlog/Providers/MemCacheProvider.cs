using Enyim.Caching;
using MicroBlog.Providers.Interfaces;

namespace MicroBlog.Providers;

public class MemCacheProvider : IMemCacheProvider
{
    public IMemcachedClient MemcachedClient { get; }

    public MemCacheProvider(IMemcachedClient memcachedClient)
    {
        MemcachedClient = memcachedClient;
    }
}