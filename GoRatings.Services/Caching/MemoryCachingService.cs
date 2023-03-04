using GoRatings.Services.Caching.Interfaces;

using Microsoft.Extensions.Caching.Memory;

namespace GoRatings.Services.Caching;

public class MemoryCachingService<K, V> : ICachingService<K, V>
{
    private readonly MemoryCacheEntryOptions memoryCacheEntryOptions;
    private readonly MemoryCache memoryCache;

    public MemoryCachingService(TimeSpan cacheExpiration, TimeSpan expirationScanFrequency)
    {
        memoryCacheEntryOptions = new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = cacheExpiration };
        memoryCache = new MemoryCache(new MemoryCacheOptions() { ExpirationScanFrequency = expirationScanFrequency });
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
        memoryCache.Remove(key);
    }
}
