using MediatR;
using Microsoft.AspNetCore.Mvc;
using Unirota.Application.ViewModels.Usuarios;

namespace Unirota.Application.Queries.Usuario;

public class ConsultarUsuarioPorIdQuery : IRequest<UsuarioViewModel>
{
    [FromRoute]
    public int Id { get; set; }
}