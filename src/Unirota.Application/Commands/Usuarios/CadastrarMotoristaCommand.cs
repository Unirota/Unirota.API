using MediatR;

namespace Unirota.Application.Commands.Usuarios
{
    public class CadastrarMotoristaCommand : IRequest<bool>
    {
        public string Habilitacao { get; set; } = string.Empty;
    }
}
