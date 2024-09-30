using MediatR;
using Microsoft.AspNetCore.Mvc;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Commands.Grupos.ObterGrupoUsuario
{
    public class ObterGrupoUsuarioCommand : IRequest<ICollection<Grupo>>
    {
        [FromRoute]
        public int Id { get; set; }
    }
}


