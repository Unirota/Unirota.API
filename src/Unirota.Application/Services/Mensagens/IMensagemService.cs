// using Unirota.Application.Commands.Mensagens;

using Unirota.Application.Commands.Mensagens;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Queries.Mensagens;
using Unirota.Application.ViewModels.Mensagens;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Services.Mensagens;

public interface IMensagemService : IScopedService
{
    Task<int> Criar(CriarMensagemCommand command, int usuarioId);
    Task<Mensagem?> ObterPorId(ConsultarMensagemPorIdQuery query, CancellationToken cancellationToken);
    Task<ICollection<ListarMensagensViewModel>> ObterPorGrupoId(int grupoId);
    Task<bool> VerificarUsuarioPertenceAoGrupo(int usuarioId, int grupoId);
    Task<bool> VerificarGrupoExiste(int grupoId);
    Task<bool> VerificarUsuarioExiste(int usuarioId);
}