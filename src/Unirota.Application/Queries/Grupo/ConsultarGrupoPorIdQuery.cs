using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unirota.Application.Queries.Grupo
{
    public class ConsultarGrupoPorIdQuery : IRequest<Unirota.Domain.Entities.Grupos.Grupo>
    {
        [FromRoute]
        public int Id { get; set; }
    }
}
