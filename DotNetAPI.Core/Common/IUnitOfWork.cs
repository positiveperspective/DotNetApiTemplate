namespace DotNetAPI.Core.Common;

public interface IUnitOfWork : IDisposable
{
    public Task SaveChangesAsync(CancellationToken cancellationToken);

    public Task SaveChangesWithCacheConfigurationResetAsync(CancellationToken cancellationToken);

    public Task SaveChangesWithAllCacheResetAsync(CancellationToken cancellationToken);
}
