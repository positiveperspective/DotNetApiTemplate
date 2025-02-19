using Microsoft.EntityFrameworkCore;

namespace DotNetAPI.Infrastructure.Database.DatabaseConfig;

internal class DatabaseService : IDatabaseService
{
    private readonly DotNetAPIContext _context;

    public DatabaseService(DotNetAPIContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(CancellationToken cancellationToken)
    {
        await _context.Database.MigrateAsync(cancellationToken);
    }
}
