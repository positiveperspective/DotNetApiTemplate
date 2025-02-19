using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace DotNetAPI.Core.Common.Cache;

public class InMemoryCache
{
    public class CacheToken
    {
        private readonly TimeSpan? _slidingExpiration;
        private readonly TimeSpan? _absoluteExpirationRelativeToNow;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public IChangeToken ExpirationToken => new CancellationChangeToken(_cts.Token);

        public CacheToken(TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpirationRelativeToNow = null)
        {
            _slidingExpiration = slidingExpiration;
            _absoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
        }

        public void Apply(ICacheEntry cacheEntry)
        {
            cacheEntry.AddExpirationToken(ExpirationToken);
            cacheEntry.SlidingExpiration = _slidingExpiration;
            cacheEntry.AbsoluteExpirationRelativeToNow = _absoluteExpirationRelativeToNow;
        }

        public void Reset()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
        }
    }

    public static readonly CacheToken ConfigsCacheToken = new CacheToken(TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(180));
    private readonly IMemoryCache _cache;

    public InMemoryCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void Invalidate()
    {
        if(_cache is MemoryCache memoryCache)
        {
            memoryCache.Compact(100);
        }
    }
}
