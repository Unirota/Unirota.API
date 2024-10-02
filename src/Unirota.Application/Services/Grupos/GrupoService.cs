using Mapster;
using Unirota.Application.Commands.Grupos;
using Unirota.Application.Persistence;
using Unirota.Application.Specifications.Grupo;
using Unirota.Application.Specifications.Grupos;
using Unirota.Application.ViewModels.Grupos;
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

    public async Task<ICollection<ListarGruposViewModel>> ObterPorUsuarioId(int usuarioId)
    {
        var gruposComoPassageiro = await _repository.ListAsync(new ConsultarGrupoComoPassageiroSpec(usuarioId));
        var gruposComoMotorista = await _repository.ListAsync(new ConsultarGrupoComoMotoristaSpec(usuarioId));
        var grupos = gruposComoPassageiro.Concat(gruposComoMotorista).ToList();
        var gruposViewModel = grupos.Adapt<List<ListarGruposViewModel>>();
        return gruposViewModel;
    }
}
