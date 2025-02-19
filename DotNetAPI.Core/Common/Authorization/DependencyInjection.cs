using Microsoft.Extensions.DependencyInjection;
using DotNetAPI.Core.Common.Authorization.CurrentUserAuthorization;
using DotNetAPI.Domain.Common.Interfaces;

namespace DotNetAPI.Core.Common.Authorization;

internal static class DependencyInjection
{
    public static IServiceCollection AddAuthorizationService(this IServiceCollection services)
    {
        services.AddScoped<CurrentUser>();
        services.AddScoped<ICurrentUser>(provider => provider.GetService<CurrentUser>()!);

        return services;
    }
}
