using MediatR;
using Microsoft.AspNetCore.Mvc;
using Unirota.Application.ViewModels;
using Unirota.Application.ViewModels.Mensagens;

namespace Unirota.Application.Queries.Mensagens;


public class ListarMensagensPorGrupoQuery : IRequest<ResultadoPaginadoViewModel<ListarMensagensViewModel>?>
{
    [FromRoute]
    public int GrupoId { get; set; }

    [FromQuery]
    public int Pagina { get; set; }

    [FromQuery]
    public int QuantidadeRegistros { get; set; }
}