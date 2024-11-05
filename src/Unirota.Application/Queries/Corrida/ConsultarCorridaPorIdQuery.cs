using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unirota.Application.Queries.Corrida;

public class ConsultarCorridaPorIdQuery : IRequest<List<Domain.Entities.Corridas.Corrida>>
{
    [FromRoute]
    public int Id { get; set; }
}


