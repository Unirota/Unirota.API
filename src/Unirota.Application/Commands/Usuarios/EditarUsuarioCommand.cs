using MediatR;
using Unirota.Application.Requests.Enderecos;
using Unirota.Application.ViewModels.Usuarios;

namespace Unirota.Application.Commands.Usuarios;

public class EditarUsuarioCommand : IRequest<UsuarioViewModel>
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string? Senha { get; set; }
    public string? ImagemUrl { get; set; }
    public EditarEnderecoRequest Endereco { get; set; }
}
