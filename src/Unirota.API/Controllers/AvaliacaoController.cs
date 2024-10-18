using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Commands.Avaliacoes;
using Unirota.Application.Queries.Avaliacao;
using Unirota.Application.Services;

namespace Unirota.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class AvaliacaoController: BaseApiController
{
    public AvaliacaoController(IServiceContext serviceContext) : base(serviceContext)
    {
        
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarAvaliacaoCommand request)
    {
        return GetResponse(await Mediator.Send(Request));
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorCorrida([FromRoute] ListarAvaliacoesPorCorridaQuery request)
    {
        return GetResponse((await Mediator.Send(request)));
    }
}