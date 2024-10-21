using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Commands.Veiculos;
using Unirota.Application.Services;

namespace Unirota.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VeiculoController: BaseApiController
{ 
    public VeiculoController(IServiceContext serviceContext) : base(serviceContext)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarVeiculosCommand request)
    {
        return GetResponse(await Mediator.Send(request));
    }

}
