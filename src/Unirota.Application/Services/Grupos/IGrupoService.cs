using Unirota.Application.Commands.Grupos;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Queries.Grupo;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Services.Grupos;

public interface IGrupoService : IScopedService
{
    public Task<int> Criar(CriarGrupoCommand dto, int motoristaId);
    public Task<int> Editar(EditarGrupoCommand request, CancellationToken cancellationToken);
    public Task<Grupo?> ObterPorId(ConsultarGrupoPorIdQuery request, CancellationToken cancellationToken);
}
