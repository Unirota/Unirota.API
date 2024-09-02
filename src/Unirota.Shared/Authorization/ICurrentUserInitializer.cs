using System.Security.Claims;

namespace Unirota.Shared.Authorization;

public interface ICurrentUserInitializer
{
    void SetCurrentUser(ClaimsPrincipal user);
    void SetCurrentUserId(int userId);
}
