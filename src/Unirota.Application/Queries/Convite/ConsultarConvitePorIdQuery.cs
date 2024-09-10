using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unirota.Application.Queries.Convite;

public class ConsultarConvitePorIdQuery : IRequest<bool>
{
    [FromRoute]
    public int Id { get; set; }
}