using Unirota.Application.Commands.Mensagens;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.ViewModels.Mensagens;

namespace Unirota.Application.Services.Mensagens;

public interface IMensagemService : IScopedService
{
    Task<int> Criar(CriarMensagemCommand command, int usuarioId);
    Task<ICollection<ListarMensagensViewModel>> ObterPorGrupoId(int grupoId);
    
  
}