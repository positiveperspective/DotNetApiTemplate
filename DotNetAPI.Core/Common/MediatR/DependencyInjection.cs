using MediatR;
using Microsoft.Extensions.DependencyInjection;
using DotNetAPI.Core.Common.Extensions;
using DotNetAPI.Core.Common.MediatR.Behaviours;
using DotNetAPI.Core.Common.MediatR.Policies;

namespace DotNetAPI.Core.Common.MediatR;

internal static class DependencyInjection
{
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizeRequestBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidateRequestBehavior<,>));

        services.AddImplementationsFromAssembly(typeof(DependencyInjection).Assembly, typeof(IAuthorizationPolicy<>), ServiceLifetime.Scoped);
        services.AddImplementationsFromAssembly(typeof(DependencyInjection).Assembly, typeof(IValidationPolicy<>), ServiceLifetime.Scoped);

        return services;
    }
}
