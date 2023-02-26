using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoRatings.Services.Caching.Interfaces;

public interface ICachingService<K, V>
{
    void Add(K key, V value);
    bool TryGetValue(K key, out V value);
    void Remove(K key);
}
