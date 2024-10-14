using MediatR;
using Unirota.Application.Common.Interfaces;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Mensagens;

namespace Unirota.Application.Queries.Mensagens;

public class ConsultarMensagemPorIdQuery : IRequest<Mensagem>
{
    public int Id { get; set; }
}