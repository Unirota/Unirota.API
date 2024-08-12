using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unirota.API.Responses;
using Unirota.Application.Services;

namespace Unirota.API.Controllers.Common;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;
    protected readonly IServiceContext _serviceContext;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected BaseApiController(IServiceContext serviceContext)
    {

       _serviceContext = serviceContext;
    }

    public IActionResult GetResponse<T>(T response)
    {
        if (_serviceContext.HasError())
        {
            return ErrorBuilder();
        }
        else if (_serviceContext.HasEntityError())
        {
            return EntityErrorBuilder();
        }
        else if (_serviceContext.HasNotification())
        {
            return NotificationBuilder(response);
        }
        else if (response is null)
        {
            return NoContent();
        }
        else
        {
            return Ok(response);
        }
    }

    private IActionResult EntityErrorBuilder()
    {
        var model = new BadRequestResponse
        {
            Instance = HttpContext.Request.Path.Value?.Replace("/api", string.Empty)
        };

        if (_serviceContext.EntityErrors.Count > 1)
        {
            model.Errors = new List<BadRequestErrorResponse>();
            foreach (var propertyErrors in _serviceContext.EntityErrors)
            {
                foreach (var error in propertyErrors.Value)
                {
                    model.Errors.Add(new BadRequestErrorResponse
                    {
                        Error = error,
                        Detail = error,
                        Property = propertyErrors.Key
                    });
                }
            }
        }

        return BadRequest(model);
    }

    private IActionResult ErrorBuilder()
    {
        var model = new BadRequestResponse
        {
            Instance = HttpContext.Request.Path.Value?.Replace("/api", string.Empty)
        };

        if (_serviceContext.Errors.Count > 1)
        {
            model.Errors = new List<BadRequestErrorResponse>();
            foreach (var error in _serviceContext.Errors)
            {
                model.Errors.Add(new BadRequestErrorResponse
                {
                    Error = error,
                    Detail = error,
                });
            }
        }
        else
        {
            model.Error = new BadRequestErrorResponse
            {
                Error = _serviceContext.Errors.FirstOrDefault(),
                Detail = _serviceContext.Errors.FirstOrDefault()
            };
        }

        return BadRequest(model);
    }

    private IActionResult NotificationBuilder<T>(T data)
    {
        var model = new MultiStatusResponse<T>(data);
        model.Notifications.AddRange(_serviceContext.Notifications);
        return StatusCode((int)HttpStatusCode.MultiStatus, model);
    }
}
