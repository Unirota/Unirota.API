using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Services;

namespace Unirota.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : BaseApiController
{
    public UsuarioController(IServiceContext serviceContext) : base(serviceContext)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarUsuarioCommand request)
    {
        return GetResponse(await Mediator.Send(request));
    }
}
