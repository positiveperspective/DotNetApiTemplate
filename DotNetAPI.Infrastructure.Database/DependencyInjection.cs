using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DotNetAPI.Core.Common.Cache;
using DotNetAPI.Core.OrderUseCases.Repositories;
using DotNetAPI.Infrastructure.Database.DatabaseConfig;
using DotNetAPI.Infrastructure.Database.Repositories;
using DotNetAPI.Core.Common;

namespace DotNetAPI.Infrastructure.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDatabase(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        services.AddScoped<IDatabaseService, DatabaseService>();
        services.AddDbContext<DotNetAPIContext>(
            options =>
            {
                if (hostEnvironment.IsDevelopment())
                {
                    ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                    options.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging();
                }

                options.UseSqlServer(configuration.GetConnectionString("DotNetAPIDB"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(600)
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)                    
                    );
            },
            ServiceLifetime.Scoped);

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<SpecificationLocalCache>();
        services.AddScoped<InMemoryCache>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
