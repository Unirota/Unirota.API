using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unirota.Application.ViewModels.Corrida;

namespace Unirota.Application.Queries.Corrida;

public class ConsultarCorridaPorIdQuery : IRequest<List<Domain.Entities.Corridas.Corrida>>
{
    [FromRoute]

    public int Id { get; set; }
}


