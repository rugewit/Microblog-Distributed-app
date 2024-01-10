using Enyim.Caching;
using Enyim.Caching.Configuration;
using MicroBlog.Models.Settings;
using MicroBlog.Providers.Interfaces;
using Microsoft.Extensions.Options;

namespace MicroBlog.Providers;

public class MemCacheProvider : IMemCacheProvider
{
    private readonly IMemcachedClient _client;

    public MemCacheProvider(IOptions<MemCacheSettings> elasticSearchSettings, IMemcachedClient client)
    {
        var addresses = elasticSearchSettings.Value.Addresses.ToList();
        var ports = elasticSearchSettings.Value.Ports.ToList();
        if (addresses.Count != ports.Count)
        {
            throw new Exception("error in MemCacheProvider: count of addresses and of ports are not the same");
        }
        var list = addresses.Zip(ports, (address, port) => new Server
        {
            Address = address,
            Port = port
        }).ToList();
        
        //services.AddEnyimMemcached(o => o.Servers = list);
        //_client = services.BuildServiceProvider().GetService<IMemcachedClient>() ?? 
        //          throw new Exception("cannot initialize memcacheProvider");
        _client = client;
    }

    public IMemcachedClient GetClient()
    {
        return _client;
    }
}