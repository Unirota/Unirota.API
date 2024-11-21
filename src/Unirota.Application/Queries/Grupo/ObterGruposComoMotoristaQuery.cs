using MediatR;
using Unirota.Application.ViewModels.Grupos;

namespace Unirota.Application.Queries.Grupo;

public class ObterGruposComoMotoristaQuery : IRequest<IEnumerable<ListarGruposParaConviteViewModel>>
{
}
