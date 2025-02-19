using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DotNetAPI.Infrastructure.Database;

public class ContextAwareMigrationsAssembly : MigrationsAssembly
{
    private readonly DbContext context;

    public ContextAwareMigrationsAssembly(
        ICurrentDbContext currentContext,
        IDbContextOptions options,
        IMigrationsIdGenerator idGenerator,
        IDiagnosticsLogger<DbLoggerCategory.Migrations> logger) : base(currentContext, options, idGenerator, logger)
    {
        context = currentContext.Context;
    }

    public override Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
    {
        var hasCtorWithDbContext = migrationClass
                .GetConstructor(new[] { typeof(DbContext) }) != null;

        if (hasCtorWithDbContext)
        {
            var instance = (Migration)Activator.CreateInstance(migrationClass.AsType(), context);
            instance.ActiveProvider = activeProvider;
            return instance;
        }

        return base.CreateMigration(migrationClass, activeProvider);
    }
}
