using Unirota.Application.Services;

namespace Unirota.Application.Handlers.Common;

public abstract class BaseRequestHandler
{
    protected IServiceContext ServiceContext { get; set; }

    public BaseRequestHandler(IServiceContext serviceContext)
    {
        ServiceContext = serviceContext;
    }
}
