namespace Unirota.Application.Services;

public class ServiceContext : IServiceContext
{
    public ServiceContext()
    {
        _notifications = new List<string>();
        _errors = new List<string>();
        _entityErrors = new Dictionary<string, List<string>>();
    }

    private readonly List<string> _notifications;
    public IReadOnlyCollection<string> Notifications => _notifications.AsReadOnly();

    private readonly List<string> _errors;
    public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();

    private readonly Dictionary<string, List<string>> _entityErrors;
    public IReadOnlyDictionary<string, List<string>> EntityErrors => _entityErrors;

    public string HeaderLocation { get; private set; }

    public ServiceContext AddHeaderLocation(string location)
    {
        HeaderLocation = location;
        return this;
    }

    public ServiceContext AddError(string message)
    {
        _errors.Add(message);
        return this;
    }

    public ServiceContext AddEntityError(string property, string message)
    {
        if (!_entityErrors.TryGetValue(property, out var errorsList))
        {
            errorsList = new List<string>();
            _entityErrors[property] = errorsList;
        }

        if (errorsList.Contains(message)) return this;

        errorsList.Add(message);
        return this;
    }

    public ServiceContext AddNotification(string message)
    {
        _notifications.Add(message);
        return this;
    }

    public ServiceContext ClearErrors()
    {
        _errors.Clear();
        return this;
    }

    public ServiceContext ClearEntityErrors()
    {
        _entityErrors.Clear();
        return this;
    }

    public ServiceContext ClearNotifications()
    {
        _notifications.Clear();
        return this;
    }

    public bool HasError() => _errors.Count > 0;

    public bool HasNotification() => _notifications.Count > 0;

    public bool HasEntityError() => _entityErrors.Count > 0;
}
