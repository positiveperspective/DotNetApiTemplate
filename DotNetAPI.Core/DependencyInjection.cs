using Microsoft.Extensions.DependencyInjection;
using DotNetAPI.Core.Common.Authorization;
using DotNetAPI.Core.Common.Cache;
using DotNetAPI.Core.OrderUseCases;
using DotNetAPI.Domain.Common.Interfaces;
using System.Reflection;

namespace DotNetAPI.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAuthorizationService();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped<IDateTime, MachineDateTime>();

        services.AddScoped<InMemoryCurrentUserCache>();
        services.AddOrderUseCases();
        return services;
    }
}
