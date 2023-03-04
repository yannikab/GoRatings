namespace GoRatings.Services.Caching.Interfaces;

public interface ICachingService<K, V>
{
    void Add(K key, V value);
    bool TryGetValue(K key, out V value);
    void Remove(K key);
}
