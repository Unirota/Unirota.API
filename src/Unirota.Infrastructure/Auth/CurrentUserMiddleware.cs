using Microsoft.AspNetCore.Http;
using Unirota.Shared.Authorization;

namespace Unirota.Infrastructure.Auth;

public class CurrentUserMiddleware : IMiddleware
{
    private readonly ICurrentUserInitializer _initializer;

    public CurrentUserMiddleware(ICurrentUserInitializer initializer) =>
        _initializer = initializer;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _initializer.SetCurrentUser(context.User);
        await next(context);
    }
}
