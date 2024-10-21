using MediatR;
using Unirota.Application.Common.Interfaces;

namespace Unirota.Application.Commands.Mensagens;

public class CriarMensagemCommand : IRequest<int>
{
    public string Conteudo { get; set; }
    public int GrupoId { get; set; }
}