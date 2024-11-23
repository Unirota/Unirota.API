
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Commands.Grupos;
using Unirota.Application.Queries.Grupo;
using Unirota.Application.Services;

namespace Unirota.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GrupoController : BaseApiController
{
    public GrupoController(IServiceContext serviceContext) : base(serviceContext)
    {
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Criar([FromBody] CriarGrupoCommand request)
    {
        return GetResponse(await Mediator.Send(request));
    }

    [HttpGet("usuario/{Id}")]
    [Authorize]
    public async Task<IActionResult> Obter(ObterGrupoUsuarioCommand request)
    {
        return GetResponse(await Mediator.Send(request));
    }

    [HttpGet("home")]
    [Authorize]
    public async Task<IActionResult> ObterHome([FromQuery] ObterGruposHomeQuery request)
    {
        return GetResponse(await Mediator.Send(request));
    }

    [HttpGet("meusgrupos")]
    [Authorize]
    public async Task<IActionResult> ObterGruposComoMotorista([FromQuery] ObterGruposComoMotoristaQuery request)
    {
        return GetResponse(await Mediator.Send(request));
    }


    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Deletar(DeletarGrupoCommand request)
    {
        return GetResponse(await Mediator.Send(request));
    }


    [HttpGet("{Id}")]
    [Authorize]
    public async Task<IActionResult> ObterPorId(ConsultarGrupoPorIdQuery request)
    {
        return GetResponse(await Mediator.Send(request));
    }
}
