namespace Unirota.Application.Common.Interfaces;

public interface ICurrentUser
{
    string Name { get; }
    int GetUserId();
    string GetUserEmail();
    bool IsAuthenticated();
}
