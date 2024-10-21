using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unirota.Application.Commands.SolicitacaoEntrada;

public class CancelarSolicitacaoEntradaGrupoCommand : IRequest<bool>
{
    [FromRoute]
    public int Id { get; set; }
}
