using GoRatings.Api.Interfaces.Services;

using Microsoft.Extensions.Caching.Memory;

namespace GoRatings.Api.Services.Caching;

public class MemoryCachingService<K, V> : ICachingService<K, V>
{
    private readonly MemoryCache memoryCache;
    private readonly MemoryCacheEntryOptions memoryCacheEntryOptions;

    public MemoryCachingService()
    {
        memoryCache = new MemoryCache(new MemoryCacheOptions()
        {
            ExpirationScanFrequency = TimeSpan.FromMinutes(Settings.Instance.CacheExpirationScanFrequencyMinutes)
        });

        memoryCacheEntryOptions = new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Settings.Instance.CacheExpirationMinutes)
        };
    }

    public void Add(K key, V value)
    {
        memoryCache.Set(key, value, memoryCacheEntryOptions);
    }

    public bool TryGetValue(K key, out V value)
    {
        return memoryCache.TryGetValue(key, out value);
    }

    public void Remove(K key)
    {
        Remove(key);
    }
}
