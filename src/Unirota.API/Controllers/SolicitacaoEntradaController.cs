using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Commands.SolicitacaoEntrada;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Queries.SolicitacoesEntrada;
using Unirota.Application.Services;

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
        command.ContextId = HttpContext.Connection.Id;
        return GetResponse(await Mediator.Send(command));
    }

    [HttpPatch("recusar/{Id}")]
    [Authorize]
    public async Task<IActionResult> Recusar(RecusarEntradaGrupoCommand command)
    {
        return GetResponse(await Mediator.Send(command));
    }
    
    [HttpPatch("cancelar/{Id}")]
    [Authorize]
    public async Task<IActionResult> Recusar(CancelarSolicitacaoEntradaGrupoCommand command)
    {
        return GetResponse(await Mediator.Send(command));
    }

    [HttpGet]
    public async Task<IActionResult> ObterSolicitacoesUsuario()
    {
        return GetResponse(await Mediator.Send(new ObterSolicitacoesUsuarioQuery()));
    }
}