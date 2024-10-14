using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Commands.Mensagens;
using Unirota.Application.Queries.Mensagens;
using Unirota.Application.Services;

namespace Unirota.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class MensagemController : BaseApiController
{
    public MensagemController(IServiceContext serviceContext) : base(serviceContext)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarMensagemCommand request)
    {
        return GetResponse(await Mediator.Send(request));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId([FromRoute] ConsultarMensagemPorIdQuery request)
    {
        return GetResponse(await Mediator.Send(request));
    }

    [HttpGet("grupo/{grupoId}")]
    public async Task<IActionResult> ListarPorGrupo([FromRoute] ListarMensagensPorGrupoQuery request)
    {
        return GetResponse(await Mediator.Send(request));
    }
}