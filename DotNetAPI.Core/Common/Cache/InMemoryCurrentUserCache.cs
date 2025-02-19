using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace DotNetAPI.Core.Common.Cache;

public class InMemoryCurrentUserCache
{
    public class CacheToken
    {
        private readonly DateTime? _absoluteExpiration;

        private CancellationTokenSource _cts = new();

        public IChangeToken ExpirationToken => new CancellationChangeToken(_cts.Token);

        public CacheToken(DateTime? absoluteExpiration = null)
        {
            _absoluteExpiration = absoluteExpiration;
        }

        public void Apply(ICacheEntry cacheEntry)
        {
            cacheEntry.AddExpirationToken(ExpirationToken);
            cacheEntry.AbsoluteExpiration = _absoluteExpiration;
        }

        public void Reset()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
        }
    }

    private static readonly int Year = DateTime.Now.Hour > 3 ? DateTime.Now.AddDays(1).Year : DateTime.Now.Year;
    private static readonly int Month = DateTime.Now.Hour > 3 ? DateTime.Now.AddDays(1).Month : DateTime.Now.Month;
    private static readonly int Day = DateTime.Now.Hour > 3 ? DateTime.Now.AddDays(1).Day : DateTime.Now.Day;

    public static readonly CacheToken UserCacheToken = new(new DateTime(Year, Month, Day, 3, 0, 0));

    public void Invalidate()
    {
        UserCacheToken.Reset();
    }
}
