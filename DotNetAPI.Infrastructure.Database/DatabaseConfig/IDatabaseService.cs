namespace DotNetAPI.Infrastructure.Database.DatabaseConfig;

public interface IDatabaseService
{
    Task CreateAsync(CancellationToken cancellationToken);
}
