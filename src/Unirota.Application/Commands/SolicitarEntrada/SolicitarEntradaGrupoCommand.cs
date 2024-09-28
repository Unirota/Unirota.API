using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unirota.Application.Commands.SolicitarEntrada;

public class SolicitarEntradaGrupoCommand : IRequest<bool>
{
    [FromRoute]
    public int GrupoId { get; set; }
    
}
