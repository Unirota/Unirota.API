using Mapster;
using Unirota.Application.Commands.Mensagens;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Grupo;
using Unirota.Application.Queries.Mensagens;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Specifications.Grupos;
using Unirota.Application.Specifications.Mensagens;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Application.ViewModels.Mensagens;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Mensagens;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Application.Services.Mensagens;

internal class MensagemService : IMensagemService
{
    private readonly IRepository<Mensagem> _mensagemRepository;
    private readonly IGrupoService _grupoService;
    private readonly IUsuarioService _usuarioService;
    private readonly IServiceContext _serviceContext;

    public MensagemService(
        IRepository<Mensagem> mensagemRepository, 
        IGrupoService grupoService,
        IUsuarioService usuarioService,
        IServiceContext serviceContext)
    {
        _mensagemRepository = mensagemRepository;
        _grupoService = grupoService;
        _usuarioService = usuarioService;
        _serviceContext = serviceContext;
    }

    public async Task<int> Criar(CriarMensagemCommand command, int usuarioId)
    {
        var grupo = await _grupoService.ObterPorId(new ConsultarGrupoPorIdQuery
        {
            Id = command.GrupoId
        }, CancellationToken.None);
        
        if (grupo is null) 
        {
            _serviceContext.AddError("Grupo não encontrado");
            return 0;
        }
    
        if (!await _usuarioService.VerificarUsuarioExiste(usuarioId))
        {
            _serviceContext.AddError("Usuário não encontrado");
            return 0;
        }

        if (!await _grupoService.VerificarUsuarioPertenceAoGrupo(usuarioId, command.GrupoId))
        {
            _serviceContext.AddError("Usuário não pertence ao grupo");
            return 0;
        }
    
        try
        {
            var mensagem = new Mensagem(command.Conteudo, usuarioId, command.GrupoId);
            await _mensagemRepository.AddAsync(mensagem);
            return mensagem.Id;
        }
        catch (ArgumentException ex)
        {
            _serviceContext.AddError(ex.Message);
            return 0;
        }
    }

    public async Task<ICollection<ListarMensagensViewModel>> ObterPorGrupoId(int grupoId)
    {
        var mensagens = await _mensagemRepository.ListAsync(new ConsultarMensagensPorGrupoSpec(grupoId));
        return mensagens.Adapt<List<ListarMensagensViewModel>>();
    }
    
}