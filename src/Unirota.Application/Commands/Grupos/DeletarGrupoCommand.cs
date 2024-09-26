using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unirota.Application.Commands.Grupos
{
    public class DeletarGrupoCommand : IRequest<bool>
    {
        [FromRoute]
        public int Id { get; set; }
    }
}
