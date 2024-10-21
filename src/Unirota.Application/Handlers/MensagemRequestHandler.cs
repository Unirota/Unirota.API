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
                                      IRequestHandler<CriarMensagemCommand, int>,
                                      IRequestHandler<ListarMensagensPorGrupoQuery, ResultadoPaginadoViewModel<ListarMensagensViewModel>?>
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

    public async Task<int> Handle(CriarMensagemCommand request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.GetUserId(); 
        return await _mensagemService.Criar(request, usuarioId);
    }

    public async Task<ResultadoPaginadoViewModel<ListarMensagensViewModel>?> Handle(ListarMensagensPorGrupoQuery request, CancellationToken cancellationToken)
    {
        if (!await _grupoService.VerificarUsuarioPertenceAoGrupo(_currentUser.GetUserId(), request.GrupoId))
        {
            ServiceContext.AddError("Usuário só pode consultar mensagens de um grupo que participa");
            return null;
        }

        var quantidadeRegistros = await _readRepository.CountAsync(new ListarMensagemBaseSpec(request.Pagina, request.QuantidadeRegistros, request.GrupoId), cancellationToken);

        var registros = await _readRepository.ListAsync(new ListarMensagemBaseSpec(request.Pagina, request.QuantidadeRegistros, request.GrupoId), cancellationToken);

        return new ResultadoPaginadoViewModel<ListarMensagensViewModel>(quantidadeRegistros, registros.Adapt<ICollection<ListarMensagensViewModel>>());
    }
}
