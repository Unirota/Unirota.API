using MediatR;
using Unirota.Application.ViewModels.Mensagens;

namespace Unirota.Application.Commands.Mensagens;

public class CriarMensagemCommand : IRequest<ListarMensagensViewModel>
{
    public string Conteudo { get; set; }
    public int GrupoId { get; set; }
}