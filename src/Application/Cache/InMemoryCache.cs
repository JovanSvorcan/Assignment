using System.Collections.Concurrent;

namespace Assignment.Application.Cache;

public static class InMemoryCache<TKey, TValue> where TKey: notnull
{
    private static readonly ConcurrentDictionary<TKey, TValue> _cacheMap = new ConcurrentDictionary<TKey, TValue>();
    private static readonly LinkedList<TKey> _recentlyUsed = new LinkedList<TKey>();
    private static readonly int _maxCapacity = 5;

    public static void AddOrUpdate(TKey key, TValue value)
    {
        _cacheMap[key] = value;

        lock (_recentlyUsed)
        {
            _recentlyUsed.Remove(key);
            _recentlyUsed.AddFirst(key);

            if (_recentlyUsed.Count > _maxCapacity)
            {
                RemoveLastUsedItem();
            }
        }
    }

    private static void RemoveLastUsedItem()
    {
        var last = _recentlyUsed.Last;
        _recentlyUsed.RemoveLast();
        if (last != null)
        {
            _cacheMap.TryRemove(last.Value, out _);
        }
    }

    public static bool TryGet(TKey key, out TValue? value)
    {
        if (_cacheMap.TryGetValue(key, out var cacheItem))
        {
            value = cacheItem;

            lock (_recentlyUsed)
            {
                _recentlyUsed.Remove(key);
                _recentlyUsed.AddFirst(key);
            }

            return true;
        }

        value = default;
        return false;
    }

    public static void Reset()
    {
        _cacheMap.Clear();
        _recentlyUsed.Clear();
    }

}
