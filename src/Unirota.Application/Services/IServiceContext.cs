using Unirota.Application.Common.Interfaces;

namespace Unirota.Application.Services;

public interface IServiceContext : IScopedService
{
    ServiceContext AddNotification(string message);
    ServiceContext AddError(string message);
    ServiceContext AddEntityError(string property, string message);
    ServiceContext ClearNotifications();
    ServiceContext ClearErrors();
    ServiceContext ClearEntityErrors();
    bool HasNotification();
    bool HasError();
    bool HasEntityError();
    IReadOnlyCollection<string> Notifications { get; }
    IReadOnlyCollection<string> Errors { get; }
    public IReadOnlyDictionary<string, List<string>> EntityErrors { get; }
}
