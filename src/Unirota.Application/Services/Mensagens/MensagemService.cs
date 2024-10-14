using Mapster;
using Unirota.Application.Commands.Mensagens;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Mensagens;
using Unirota.Application.Specifications.Grupos;
using Unirota.Application.Specifications.Mensagens;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Application.ViewModels.Mensagens;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Usuarios;

namespace Unirota.Application.Services.Mensagens;

internal class MensagemService : IMensagemService
{
    private readonly IRepository<Mensagem> _mensagemRepository;
    private readonly IRepository<Grupo> _grupoRepository;
    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly IServiceContext _serviceContext;

    public MensagemService(
        IRepository<Mensagem> mensagemRepository, 
        IRepository<Grupo> grupoRepository,
        IRepository<Usuario> usuarioRepository,
        IServiceContext serviceContext)
    {
        _mensagemRepository = mensagemRepository;
        _grupoRepository = grupoRepository;
        _usuarioRepository = usuarioRepository;
        _serviceContext = serviceContext;
    }

    public async Task<int> Criar(CriarMensagemCommand command, int usuarioId)
    {
        if (!await VerificarGrupoExiste(command.GrupoId))
        {
            _serviceContext.AddError("Grupo não encontrado");
            return 0;
        }
    
        if (!await VerificarUsuarioExiste(usuarioId))
        {
            _serviceContext.AddError("Usuário não encontrado");
            return 0;
        }

        if (!await VerificarUsuarioPertenceAoGrupo(usuarioId, command.GrupoId))
        {
            _serviceContext.AddError("Usuário não pertence ao grupo");
            return 0;
        }
    
        try
        {
            var mensagem = Mensagem.Criar(command.Conteudo, usuarioId, command.GrupoId);
            await _mensagemRepository.AddAsync(mensagem);
            return mensagem.Id;
        }
        catch (ArgumentException ex)
        {
            _serviceContext.AddError(ex.Message);
            return 0;
        }
    }

    public async Task<Mensagem?> ObterPorId(ConsultarMensagemPorIdQuery query, CancellationToken cancellationToken)
    {
        var mensagem = await _mensagemRepository.FirstOrDefaultAsync(
            new ConsultarMensagemPorIdSpec(query.Id), cancellationToken);
    
        if (mensagem == null)
            _serviceContext.AddError("Mensagem não encontrada");
    
        return mensagem;
    }

    public async Task<ICollection<ListarMensagensViewModel>> ObterPorGrupoId(int grupoId)
    {
        var mensagens = await _mensagemRepository.ListAsync(new ConsultarMensagensPorGrupoSpec(grupoId));
        return mensagens.Adapt<List<ListarMensagensViewModel>>();
    }
    
    public async Task<bool> VerificarUsuarioPertenceAoGrupo(int usuarioId, int grupoId)
    {
        var grupo = await _grupoRepository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(grupoId));
        return grupo?.Passageiros.Any(p => p.UsuarioId == usuarioId) ?? false;
    }
    
    public async Task<bool> VerificarGrupoExiste(int grupoId)
    {
        var grupo = await _grupoRepository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(grupoId));
        return grupo != null;
    }

    public async Task<bool> VerificarUsuarioExiste(int usuarioId)
    {
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(usuarioId));
        return usuario != null;
    }
}