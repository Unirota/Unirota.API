using MediatR;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Persistence;
using Unirota.Application.Requests.Usuarios;
using Unirota.Application.Services;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Application.Handlers;

public class UsuarioRequestHandler : BaseRequestHandler,
                                     IRequestHandler<CriarUsuarioCommand, int>
{
    private IRepository<Usuario>

    public UsuarioRequestHandler(IServiceContext serviceContext) : base(serviceContext)
    {
    }

    public async Task<int> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
    {
        return 0;
    }
}
