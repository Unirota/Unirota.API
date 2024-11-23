using MediatR;
using Unirota.Application.ViewModels.Grupos;

namespace Unirota.Application.Queries.SolicitacoesEntrada;

public class ObterSolicitacoesUsuarioQuery : IRequest<ICollection<ListarGruposViewModel>>
{
}
