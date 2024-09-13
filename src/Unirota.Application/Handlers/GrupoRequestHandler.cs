using MediatR;
using Unirota.Application.Commands.Grupos;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Persistence;
using Unirota.Application.Services;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Application.Handlers;

public class GrupoRequestHandler : BaseRequestHandler,
                                   IRequestHandler<CriarGrupoCommand, int>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Usuario> _readUserRepository;
    private readonly IGrupoService _service;

    public GrupoRequestHandler(IServiceContext serviceContext,
                               ICurrentUser currentUser,
                               IGrupoService service,
                               IReadRepository<Usuario> readUserRepository) : base(serviceContext)
    {
        _currentUser = currentUser;
        _readUserRepository = readUserRepository;
        _service = service;
    }

    public async Task<int> Handle(CriarGrupoCommand request, CancellationToken cancellationToken)
    {
        var motorista = await _readUserRepository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(_currentUser.GetUserId()), cancellationToken);
        if (motorista is null)
        {
            ServiceContext.AddError("Motorista não encontrado");
            return default;
        }

        if (motorista.Habilitacao is null)
        {
            ServiceContext.AddError("Motorista informado não possui habilitação cadastrada");
            return default;
        }

        var grupo = await _service.Criar(request, motorista.Id);
        return grupo;
    }
}
