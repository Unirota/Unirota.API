using MediatR;

namespace Unirota.Application.Commands.Usuarios;

public class CriarConviteCommand : IRequest<int>
{
    public int UsuarioId { get; set; }
    public int MotoristaId { get; set; }
    public int GrupoId { get; set; }
}
