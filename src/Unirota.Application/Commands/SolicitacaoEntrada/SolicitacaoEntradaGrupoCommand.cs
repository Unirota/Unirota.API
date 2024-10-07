using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unirota.Application.Commands.SolicitacaoEntrada;

public class SolicitacaoEntradaGrupoCommand : IRequest<bool>
{
    [FromRoute]
    public int GrupoId { get; set; }
    
}
