using Mapster;
// using Unirota.Application.Commands.Mensagens;
using Unirota.Application.Persistence;
// using Unirota.Application.Queries.Mensagens;
// using Unirota.Application.Specifications.Mensagens;
// using Unirota.Application.ViewModels.Mensagens;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Services.Mensagens;

internal class MensagemService : IMensagemService
{
    private readonly IRepository<Mensagem> _mensagemRepository;
    private readonly IRepository<Grupo> _grupoRepository;
    private readonly IServiceContext _serviceContext;

    public MensagemService(
        IRepository<Mensagem> mensagemRepository, 
        IRepository<Grupo> grupoRepository,
        IServiceContext serviceContext)
    {
        _mensagemRepository = mensagemRepository;
        _grupoRepository = grupoRepository;
        _serviceContext = serviceContext;
    }

    // public async Task<int> Criar(CriarMensagemCommand command, int usuarioId)
    // {
    //     if (!await VerificarGrupoExiste(command.GrupoId))
    //     {
    //         _serviceContext.AddError("Grupo não encontrado");
    //         return 0;
    //     }
    //
    //     if (!await VerificarUsuarioPertenceAoGrupo(usuarioId, command.GrupoId))
    //     {
    //         _serviceContext.AddError("Usuário não pertence ao grupo");
    //         return 0;
    //     }
    //
    //     var mensagem = new Mensagem(command.Conteudo, usuarioId, command.GrupoId);
    //     await _mensagemRepository.AddAsync(mensagem);
    //     return mensagem.Id;
    // }

    // public async Task<Mensagem?> ObterPorId(ConsultarMensagemPorIdQuery query, CancellationToken cancellationToken)
    // {
    //     var mensagem = await _mensagemRepository.FirstOrDefaultAsync(
    //         new ConsultarMensagemPorIdSpec(query.Id), cancellationToken);
    //
    //     if (mensagem == null)
    //         _serviceContext.AddError("Mensagem não encontrada");
    //
    //     return mensagem;
    // }

    // public async Task<ICollection<ListarMensagensViewModel>> ObterPorGrupoId(int grupoId)
    // {
    //     var mensagens = await _mensagemRepository.ListAsync(new ConsultarMensagensPorGrupoSpec(grupoId));
    //     return mensagens.Adapt<List<ListarMensagensViewModel>>();
    // }
    //
    // public async Task<bool> VerificarUsuarioPertenceAoGrupo(int usuarioId, int grupoId)
    // {
    //     var grupo = await _grupoRepository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(grupoId));
    //     return grupo?.Passageiros.Any(p => p.UsuarioId == usuarioId) ?? false;
    // }
    //
    // public async Task<bool> VerificarGrupoExiste(int grupoId)
    // {
    //     var grupo = await _grupoRepository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(grupoId));
    //     return grupo != null;
    // }
}