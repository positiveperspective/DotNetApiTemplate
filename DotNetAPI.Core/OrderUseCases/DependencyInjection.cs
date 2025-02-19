using Microsoft.Extensions.DependencyInjection;
using DotNetAPI.Core.OrderUseCases.Validation;

namespace DotNetAPI.Core.OrderUseCases;

internal static class DependencyInjection
{
    public static IServiceCollection AddOrderUseCases(this IServiceCollection services)
    {
        services.AddScoped<IOrderValidationService, OrderValidationService>();

        return services;
    }
}
