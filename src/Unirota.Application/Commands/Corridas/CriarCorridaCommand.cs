using MediatR;

namespace Unirota.Application.Commands.Corridas;

public class CriarCorridaCommand : IRequest<int>
{
    public int GrupoId { get; set; }
}

