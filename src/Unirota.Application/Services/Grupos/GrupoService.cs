using Unirota.Application.Commands.Grupos;
using Unirota.Application.Persistence;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Commands.Grupos.ObterGrupoUsuario;

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
    

    public async Task<bool> Deletar(DeletarGrupoCommand dto, Grupo grupo)
    {
        await _repository.DeleteAsync(grupo);
        return true;
    }

    public async Task<ICollection<Grupo>> ObterPorUsuarioId(int usuarioId)
    {
        var grupos = await _repository.ListAsync(new ConsultarGrupoPorUsuarioIdSpec(usuarioId));
        return grupos;
    }
}
