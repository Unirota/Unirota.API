using MediatR;
using Microsoft.AspNetCore.Mvc;
using Unirota.Application.ViewModels.Grupos;

namespace Unirota.Application.Commands.Grupos;

public class ObterGrupoUsuarioCommand : IRequest<ICollection<ListarGruposViewModel>>
{
    [FromRoute]
    public int Id { get; set; }
}


