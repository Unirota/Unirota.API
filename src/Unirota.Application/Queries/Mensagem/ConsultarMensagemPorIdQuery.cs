using MediatR;
using Unirota.Application.Common.Interfaces;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Queries.Mensagens;

public class ConsultarMensagemPorIdQuery : IRequest<Mensagem>
{
    public int Id { get; set; }
}