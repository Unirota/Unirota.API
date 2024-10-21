using MediatR;

namespace Unirota.Application.Commands.Avaliacoes;

public class CriarAvaliacaoCommand : IRequest<int>
{
    public int Nota { get; set; }
    public int CorridaId { get; set; }
}