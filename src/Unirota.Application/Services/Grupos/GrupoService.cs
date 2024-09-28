using Unirota.Application.Commands.Grupos;
using Unirota.Application.Persistence;
using Unirota.Application.Specifications.Grupos;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;

namespace Unirota.Application.Services.Grupos;

internal class GrupoService : IGrupoService
{
    private readonly IRepository<Grupo> _repository;

    public GrupoService(IRepository<Grupo> repository, IRepository<SolicitacaoDeEntrada> solicitacaoRepository)
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

    public async Task<bool> VerificarUsuarioPertenceAoGrupo(int usuarioId, int grupoId)
    {
        var grupo = await _repository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(grupoId));
        return grupo?.Passageiros.Any(p => p.UsuarioId == usuarioId) ?? false;
    }
    
    public async Task<bool> VerificarGrupoAtingiuLimiteUsuarios(int grupoId)
    {
        var grupo = await _repository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(grupoId));
        return grupo != null && grupo.Passageiros.Count >= grupo.PassageiroLimite;
    }
    
    public async Task<bool> VerificarGrupoExiste(int grupoId)
    {
        var grupo = await _repository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(grupoId));
        return grupo != null;
    }
    
}
