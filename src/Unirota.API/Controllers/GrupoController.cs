
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Commands.Grupos;
using Unirota.Application.Commands.Grupos.ObterGrupoUsuario;
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

    [HttpGet("/Usuario/{Id}")]
    [Authorize]
    public async Task<IActionResult> Obter(ObterGrupoUsuarioCommand request)
    {
        return GetResponse(await Mediator.Send(request));
    }

}
