using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unirota.Application.Commands.Convites;

public class AceitarConviteCommand : IRequest<bool>
{
    [FromRoute]
    public int Id { get; set; }
}