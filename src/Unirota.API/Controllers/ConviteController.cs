using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Services;

namespace Unirota.API.Controllers;

[Route("api/[controller]")]
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
}
