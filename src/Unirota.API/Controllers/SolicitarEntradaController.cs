using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Commands.SolicitarEntrada;
using Unirota.Application.Services;
using Unirota.Application.Common.Interfaces;

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
    public async Task<IActionResult> SolicitarEntrada([FromBody] SolicitarEntradaGrupoCommand command)
    {
        return GetResponse(await Mediator.Send(command));
    }
}