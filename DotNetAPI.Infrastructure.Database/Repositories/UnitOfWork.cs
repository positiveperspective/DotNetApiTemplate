using DotNetAPI.Core.Common;
using DotNetAPI.Core.Common.Cache;

namespace DotNetAPI.Infrastructure.Database.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DotNetAPIContext _context;

    public UnitOfWork(DotNetAPIContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesWithCacheConfigurationResetAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
        InMemoryCache.ConfigsCacheToken.Reset();
    }

    public async Task SaveChangesWithAllCacheResetAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
        InMemoryCache.ConfigsCacheToken.Reset();
        InMemoryCurrentUserCache.UserCacheToken.Reset();
    }
}
