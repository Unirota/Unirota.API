using MediatR;
using Unirota.Application.ViewModels.Grupos;

namespace Unirota.Application.Queries.Grupo;

public class ObterGruposHomeQuery : IRequest<ICollection<ListarGruposViewModel>>
{
    public string? Destino { get; set; }
}
