using Unirota.Application.Commands.Mensagens;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.ViewModels.Mensagens;
using Unirota.Domain.Entities.Mensagens;

namespace Unirota.Application.Services.Mensagens;

public interface IMensagemService : IScopedService
{
    Task<Mensagem> Criar(CriarMensagemCommand command, int usuarioId);
    Task<ICollection<ListarMensagensViewModel>> ObterPorGrupoId(int grupoId);
    
  
}