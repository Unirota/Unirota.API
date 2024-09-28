using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unirota.Application.Commands.Grupos.ObterGrupoUsuario
{
    public class ObterGrupoUsuarioCommand : IRequest<string>
    {
        [FromRoute]
        public int Id { get; set; }
    }
}
