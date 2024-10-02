using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Services;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Commands.SolicitacaoEntrada;

namespace Unirota.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SolicitacaoEntradaController : BaseApiController
{
    private readonly ICurrentUser _currentUser;

    public SolicitacaoEntradaController(IServiceContext serviceContext, ICurrentUser currentUser) : base(serviceContext)
    {
        _currentUser = currentUser;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SolicitarEntrada([FromBody] SolicitacaoEntradaGrupoCommand command)
    {
        return GetResponse(await Mediator.Send(command));
    }

    [HttpPatch("aceitar/{Id}")]
    [Authorize]
    public async Task<IActionResult> Aceitar(AceitarEntradaGrupoCommand command)
    {
        return GetResponse(await Mediator.Send(command));
    }
}