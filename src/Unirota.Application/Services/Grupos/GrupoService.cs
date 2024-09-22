using Unirota.Application.Commands.Grupos;
using Unirota.Application.Persistence;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Services.Grupos;

internal class GrupoService : IGrupoService
{
    private readonly IRepository<Grupo> _repository;

    public GrupoService(IRepository<Grupo> repository)
    {
        _repository = repository;
    }

    public async Task<int> Criar(CriarGrupoCommand dto, int motoristaId)
    {
        var grupo = new Grupo(dto.Nome, dto.PassageiroLimite, dto.HoraInicio, motoristaId);

        if (!string.IsNullOrEmpty(dto.Descricao))
        {
            grupo.AlterarDescricao(dto.Descricao);
        }

        if (!string.IsNullOrEmpty(dto.ImagemUrl))
        {
            grupo.AlterarImagem(dto.ImagemUrl);
        }

        await _repository.AddAsync(grupo);
        return grupo.Id;
    }
}
