using MediatR;
using Unirota.Application.ViewModels.Grupos;

namespace Unirota.Application.Queries.Convites;

public class ObterConvitesUsuarioQuery : IRequest<ICollection<ListarGruposViewModel>>
{
}
