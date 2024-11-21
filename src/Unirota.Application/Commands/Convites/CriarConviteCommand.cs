using MediatR;

namespace Unirota.Application.Commands.Convites;

public class CriarConviteCommand : IRequest<int>
{
    public string Email { get; set; }
    public int GrupoId { get; set; } 
}
