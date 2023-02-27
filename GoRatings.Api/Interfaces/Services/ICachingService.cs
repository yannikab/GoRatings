namespace GoRatings.Api.Interfaces.Services;

public interface ICachingService<K, V>
{
    void Add(K key, V value);
    bool TryGetValue(K key, out V value);
    void Remove(K key);
}
