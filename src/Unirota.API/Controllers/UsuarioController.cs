using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Queries.Usuario;
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

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Editar([FromRoute] int id, EditarUsuarioCommand request)
    {
        request.Id = id;
        return GetResponse(await Mediator.Send(request));
    }

   [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(ConsultarUsuarioPorIdQuery request)
    {
        return GetResponse(await Mediator.Send(request));
    }
}
