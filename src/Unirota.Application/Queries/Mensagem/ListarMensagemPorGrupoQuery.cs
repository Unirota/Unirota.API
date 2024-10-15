using MediatR;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.ViewModels.Mensagens;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Queries.Mensagens;


public class ListarMensagensPorGrupoQuery : IRequest<IEnumerable<ListarMensagensViewModel>>
{
    public int GrupoId { get; set; }
}