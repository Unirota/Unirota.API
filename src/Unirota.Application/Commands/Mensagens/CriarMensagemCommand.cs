using MediatR;
using Unirota.Domain.Entities.Mensagens;

namespace Unirota.Application.Commands.Mensagens;

public class CriarMensagemCommand : IRequest<Mensagem>
{
    public string Conteudo { get; set; }
    public int GrupoId { get; set; }
}