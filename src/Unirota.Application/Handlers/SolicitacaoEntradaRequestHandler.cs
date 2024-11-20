using MediatR;
using Unirota.Application.Commands.SolicitacaoEntrada;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.SolicitacoesEntrada;
using Unirota.Application.Services;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Services.SolicitacaoEntrada;
using Unirota.Application.Specifications.Convites;
using Unirota.Application.Specifications.Grupos;
using Unirota.Application.Specifications.SolicitacaoEntrada;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Application.ViewModels.Grupos;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Application.Handlers;

public class SolicitacaoEntradaRequestHandler : BaseRequestHandler,
                                                IRequestHandler<SolicitacaoEntradaGrupoCommand, bool>,
                                                IRequestHandler<AceitarEntradaGrupoCommand, bool>,
                                                IRequestHandler<RecusarEntradaGrupoCommand, bool>,
                                                IRequestHandler<CancelarSolicitacaoEntradaGrupoCommand, bool>,
                                                IRequestHandler<ObterSolicitacoesUsuarioQuery, ICollection<ListarGruposViewModel>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Usuario> _readUserRepository;
    private readonly IReadRepository<Grupo> _readGrupoRepository;
    private readonly IReadRepository<SolicitacaoDeEntrada> _readRepository;
    private readonly IGrupoService _grupoService;
    private readonly ISolicitacaoEntradaService _solicitacaoEntradaService;


    public SolicitacaoEntradaRequestHandler(IServiceContext serviceContext,
                                          ICurrentUser currentUser,
                                          IGrupoService grupoService,
                                          ISolicitacaoEntradaService solicitacaoEntradaService,
                                          IReadRepository<Usuario> readUserRepository,
                                          IReadRepository<Grupo> readGrupoRepository,
                                          IReadRepository<SolicitacaoDeEntrada> readRepository) : base(serviceContext)
    
    {
        _currentUser = currentUser;
        _readUserRepository = readUserRepository;
        _readGrupoRepository = readGrupoRepository;
        _grupoService = grupoService;
        _solicitacaoEntradaService = solicitacaoEntradaService;
        _readRepository = readRepository;
    }

    public async Task<bool> Handle(SolicitacaoEntradaGrupoCommand request, CancellationToken cancellationToken)
    {
        if (request.GrupoId == 0)
        {
            ServiceContext.AddError("GrupoId inválido");
            return false;
        }
        
        var usuario = await _readUserRepository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(_currentUser.GetUserId()), cancellationToken);
        if (usuario is null)
        {
            ServiceContext.AddError("Usuário não encontrado");
            return false;
        }

        var grupo = await _readGrupoRepository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(request.GrupoId), cancellationToken);
        if (grupo is null)
        {
            ServiceContext.AddError("Grupo não encontrado");
            return false;
        }

        if (await _grupoService.VerificarUsuarioPertenceAoGrupo(usuario.Id, grupo.Id))
        {
            ServiceContext.AddError("O usuário já pertence a este grupo");
            return false;
        }

        if (await _grupoService.VerificarGrupoAtingiuLimiteUsuarios(grupo.Id))
        {
            ServiceContext.AddError("O grupo atingiu o limite de usuários");
            return false;
        }
        
        

        return await _solicitacaoEntradaService.CriarSolicitacaoEntrada(usuario.Id, grupo.Id);
    }

    public async Task<bool> Handle(AceitarEntradaGrupoCommand request, CancellationToken cancellationToken)
    {
        return await _solicitacaoEntradaService.AceitarSolicitacaoEntrada(request.Id, request.ContextId, cancellationToken);
    }

    public async Task<bool> Handle(RecusarEntradaGrupoCommand request, CancellationToken cancellationToken)
    {
        return await _solicitacaoEntradaService.RecusarSolicitacaoEntrada(request.Id, cancellationToken);
    }

    public async Task<bool> Handle(CancelarSolicitacaoEntradaGrupoCommand request, CancellationToken cancellationToken)
    {
        return await _solicitacaoEntradaService.CancelarSolicitacaoEntrada(request.Id, cancellationToken);
    }

    public async Task<ICollection<ListarGruposViewModel>> Handle(ObterSolicitacoesUsuarioQuery request, CancellationToken cancellationToken)
    {
        var solicitacao = await _readRepository.ListAsync(new ObterSolicitacaoPorUsuarioIdSpec(_currentUser.GetUserId()), cancellationToken);

        if (solicitacao is null)
        {
            return [];
        }

        if (solicitacao.Count == 0)
        {
            return [];
        }

        return solicitacao.Select(x => new ListarGruposViewModel
        {
            Id = x.Id,
            Nome = x.Grupo.Nome,
            UltimaMensagem = x.Grupo.Mensagens.Count > 0 ? x.Grupo.Mensagens.OrderByDescending(y => y.CreatedAt).First().Conteudo : "",
            Descricao = x.Grupo.Descricao ?? "",
            Motorista = x.Grupo.Motorista?.Nome ?? "",
            Destino = x.Grupo.Destino,
            HoraInicio = x.Grupo.HoraInicio,
            Participantes = x.Grupo.Passageiros.Count,
            DataCriacao = x.Grupo.CreatedAt,
            Nota = 5.0
        }).DistinctBy(x => x.Id).ToList();
    }
}