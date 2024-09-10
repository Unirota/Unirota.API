using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Queries.Convite;
using Unirota.Application.Services;

namespace Unirota.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ConviteController : BaseApiController
{
    public ConviteController(IServiceContext serviceContext) : base(serviceContext)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarConviteCommand request)
    {
        return GetResponse(await Mediator.Send(request));
    }

    [HttpDelete("cancelar/{id}")]
    public async Task<IActionResult> Cancelar(ConsultarConvitePorIdQuery request)
    {
        return GetResponse(await Mediator.Send(request));
    }
}
