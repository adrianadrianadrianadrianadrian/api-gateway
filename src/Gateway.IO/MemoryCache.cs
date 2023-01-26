namespace Gateway.IO;

using Bearded.Monads;
using Microsoft.Extensions.Caching.Memory;
using System;

public class MemoryCache<T> : ICache<T>
{
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _expiry;

    public MemoryCache(IMemoryCache cache, int expiryInSeconds)
    {
        _cache = cache;
        _expiry = TimeSpan.FromSeconds(expiryInSeconds);
    }

    public Option<T> Get(string id)
    {
        if (_cache.TryGetValue(id, out var t) && t is T)
            return (T)t;

        return Option<T>.None;
    }

    public void Set(string id, T t)
    {
        _cache.Set(id, t, _expiry);
    }
}