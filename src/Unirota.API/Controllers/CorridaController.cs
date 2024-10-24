﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unirota.API.Controllers.Common;
using Unirota.Application.Commands.Corridas;
using Unirota.Application.Queries.Corrida;
using Unirota.Application.Services;

namespace Unirota.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CorridaController : BaseApiController
{
    public CorridaController(IServiceContext serviceContext) : base(serviceContext)
    {
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Criar([FromBody] CriarCorridaCommand request)
    {   
        return GetResponse(await Mediator.Send(request));
    }

    
    [Authorize]
    [HttpGet("{Id}")]
    public async Task<IActionResult> Obter (ConsultarCorridaPorIdQuery request)
    {
        return GetResponse(await Mediator.Send(request));
    }
}

