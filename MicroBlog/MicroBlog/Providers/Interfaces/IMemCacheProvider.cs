using Enyim.Caching;

namespace MicroBlog.Providers.Interfaces;

public interface IMemCacheProvider
{
   public IMemcachedClient MemcachedClient { get; }
}