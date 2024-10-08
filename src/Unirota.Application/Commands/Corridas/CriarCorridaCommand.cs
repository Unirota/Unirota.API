using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unirota.Application.Commands.Corridas;

public class CriarCorridaCommand : IRequest<int>
{
    public int GrupoId { get; set; }
}

