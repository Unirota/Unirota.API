using MediatR;
using Unirota.Application.Commands.Grupos;
using Unirota.Application.Commands.SolicitarEntrada;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers.Common;
using Unirota.Application.Persistence;
using Unirota.Application.Services;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Services.SolicitacaoEntrada;
using Unirota.Application.Specifications.Grupos;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Domain.Entities.Usuarios;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Handlers;

public class SolicitarEntradaRequestHandler : BaseRequestHandler,
                                              IRequestHandler<SolicitarEntradaGrupoCommand, bool>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Usuario> _readUserRepository;
    private readonly IReadRepository<Grupo> _readGrupoRepository;
    private readonly IGrupoService _grupoService;
    private readonly ISolicitacaoEntradaService _solicitacaoEntradaService;


    public SolicitarEntradaRequestHandler(IServiceContext serviceContext,
                                          ICurrentUser currentUser,
                                          IGrupoService grupoService,
                                          ISolicitacaoEntradaService solicitacaoEntradaService,
                                          IReadRepository<Usuario> readUserRepository,
                                          IReadRepository<Grupo> readGrupoRepository) : base(serviceContext)
    
    {
        _currentUser = currentUser;
        _readUserRepository = readUserRepository;
        _readGrupoRepository = readGrupoRepository;
        _grupoService = grupoService;
        _solicitacaoEntradaService = solicitacaoEntradaService;
    }

    public async Task<bool> Handle(SolicitarEntradaGrupoCommand request, CancellationToken cancellationToken)
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
}