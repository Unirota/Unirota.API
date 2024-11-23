using Mapster;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Unirota.Application.Commands.Mensagens;
using Unirota.Application.Hubs;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Grupo;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Specifications.Mensagens;
using Unirota.Application.ViewModels.Mensagens;
using Unirota.Domain.Entities.Mensagens;

namespace Unirota.Application.Services.Mensagens;

public class MensagemService : IMensagemService
{
    private readonly IRepository<Mensagem> _mensagemRepository;
    private readonly IGrupoService _grupoService;
    private readonly IUsuarioService _usuarioService;
    private readonly IServiceContext _serviceContext;
    private readonly IHubContext<ChatHub> _chatHub;

    public MensagemService(
        IRepository<Mensagem> mensagemRepository, 
        IGrupoService grupoService,
        IUsuarioService usuarioService,
        IServiceContext serviceContext,
        IHubContext<ChatHub> chathub)
    {
        _mensagemRepository = mensagemRepository;
        _grupoService = grupoService;
        _usuarioService = usuarioService;
        _serviceContext = serviceContext;
        _chatHub = chathub;
    }

    public async Task<ListarMensagensViewModel?> Criar(CriarMensagemCommand command, int usuarioId)
    {
        var grupo = await _grupoService.ObterPorId(new ConsultarGrupoPorIdQuery
        {
            Id = command.GrupoId
        }, CancellationToken.None);
        
        if (grupo is null) 
        {
            _serviceContext.AddError("Grupo não encontrado");
            return default;
        }
    
        if (!await _usuarioService.VerificarUsuarioExiste(usuarioId))
        {
            _serviceContext.AddError("Usuário não encontrado");
            return default;
        }

        if (!await _grupoService.VerificarUsuarioPertenceAoGrupo(usuarioId, command.GrupoId))
        {
            _serviceContext.AddError("Usuário não pertence ao grupo");
            return default;
        }

        try
        {
            var mensagem = new Mensagem(command.Conteudo, usuarioId, command.GrupoId);
            var usuarioMsg = await _usuarioService.ConsultarPorId(usuarioId, CancellationToken.None);
            await _mensagemRepository.AddAsync(mensagem);
            await _chatHub.Clients.Group(command.GrupoId.ToString()).SendAsync("ReceiveMessage", usuarioId, usuarioMsg?.Nome, command.Conteudo);
            return mensagem.Adapt<ListarMensagensViewModel>();
        }
        catch (Exception ex)
        {
            _serviceContext.AddError(ex.Message);
            return default;
        }
    }

    public async Task<ICollection<ListarMensagensViewModel>> ObterPorGrupoId(int grupoId)
    {
        var mensagens = await _mensagemRepository.ListAsync(new ConsultarMensagensPorGrupoSpec(grupoId));
        return mensagens.Adapt<List<ListarMensagensViewModel>>();
    }
    
}