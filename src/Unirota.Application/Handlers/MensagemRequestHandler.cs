using Mapster;
using MediatR;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Mensagens;
using Unirota.Application.Services;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Services.Mensagens;
using Unirota.Application.Specifications.Mensagens;
using Unirota.Application.ViewModels;
using Unirota.Application.ViewModels.Mensagens;
using Unirota.Domain.Entities.Mensagens;

namespace Unirota.Application.Commands.Mensagens;

public class MensagemRequestHandler : BaseRequestHandler,
                                      IRequestHandler<CriarMensagemCommand, ListarMensagensViewModel>,
                                      IRequestHandler<ListarMensagensPorGrupoQuery, ICollection<ListarMensagensViewModel>?>
{
    private readonly IMensagemService _mensagemService;
    private readonly IGrupoService _grupoService;
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Mensagem> _readRepository;

    public MensagemRequestHandler(IMensagemService mensagemService,
                                  IGrupoService grupoService,
                                  ICurrentUser currentUser,
                                  IReadRepository<Mensagem> readRepository,
                                  IServiceContext serviceContext) : base(serviceContext)
    {
        _mensagemService = mensagemService;
        _grupoService = grupoService;
        _currentUser = currentUser;
        _readRepository = readRepository;
    }

    public async Task<ListarMensagensViewModel> Handle(CriarMensagemCommand request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.GetUserId(); 
        return await _mensagemService.Criar(request, usuarioId);
    }

    public async Task<ICollection<ListarMensagensViewModel>?> Handle(ListarMensagensPorGrupoQuery request, CancellationToken cancellationToken)
    {
        if (!await _grupoService.VerificarUsuarioPertenceAoGrupo(_currentUser.GetUserId(), request.GrupoId))
        {
            ServiceContext.AddError("Usuário só pode consultar mensagens de um grupo que participa");
            return null;
        }

        var registros = await _readRepository.ListAsync(new ListarMensagemBaseSpec(request.GrupoId), cancellationToken);

        return registros.Adapt<ICollection<ListarMensagensViewModel>>();
    }
}
