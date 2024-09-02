using System.Security.Claims;
using Unirota.Application.Common.Interfaces;
using Unirota.Shared.Authorization;

namespace Unirota.Infrastructure.Auth;

public class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
    private ClaimsPrincipal _user;

    public string Name => _user?.Identity?.Name;
    
    private int _userId;

    public CurrentUser()
    {
    }

    public string GetUserEmail() =>
        IsAuthenticated() ?
            _user!.FindFirstValue(ClaimTypes.Email) : string.Empty;

    public int GetUserId() =>
        IsAuthenticated() ?
            int.Parse(_user.FindFirstValue(ClaimTypes.NameIdentifier)) : _userId;

    public bool IsAuthenticated() =>
        _user?.Identity?.IsAuthenticated is true;

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetCurrentUserId(int userId)
    {
        if (_userId != default)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _userId = userId;
    }
}
