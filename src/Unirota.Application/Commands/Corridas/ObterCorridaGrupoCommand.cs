using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unirota.Application.ViewModels.Corrida;

namespace Unirota.Application.Commands.Corridas;

public class ObterCorridaGrupoCommand : IRequest<ICollection<ListarCorridaViewModel>>
{
    [FromRoute]
    public int Id { get; set; }
}
