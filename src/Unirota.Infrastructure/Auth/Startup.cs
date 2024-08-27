using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Unirota.Application.Common.Interfaces;
using Unirota.Shared.Authorization;

namespace Unirota.Infrastructure.Auth;

public static class Startup
{
    internal static IServiceCollection AddAuth(this IServiceCollection services)
    {
        return services
                .AddCurrentUser();
    }

    public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
        app.UseMiddleware<CurrentUserMiddleware>();

    private static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
        services
            .AddScoped<CurrentUserMiddleware>()
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());
}
