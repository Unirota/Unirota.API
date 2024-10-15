using Mapster;
using Unirota.Application.Commands.Grupos;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Grupo;
using Unirota.Application.Specifications.Grupos;
using Unirota.Application.ViewModels.Grupos;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Services.Grupos;

internal class GrupoService : IGrupoService
{
    private readonly IRepository<Grupo> _repository;
    private readonly IServiceContext _serviceContext;

    public GrupoService(IRepository<Grupo> repository, IServiceContext serviceContext)
    {
        _repository = repository;
        _serviceContext = serviceContext;
    }

    public async Task<int> Criar(CriarGrupoCommand dto, int motoristaId)
    {
        var grupo = new Grupo(dto.Nome, dto.PassageiroLimite, dto.HoraInicio, motoristaId, dto.Destino);

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

    public Task<int> Editar(EditarGrupoCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Grupo?> ObterPorId(ConsultarGrupoPorIdQuery request, CancellationToken cancellationToken)
    {
        Grupo? grupo = await _repository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(request.Id), cancellationToken);
        
        if(grupo == null)
            _serviceContext.AddError("Grupo não encontrado");
        return grupo;
    }

    public async Task<bool> VerificarUsuarioPertenceAoGrupo(int usuarioId, int grupoId)
    {
        var grupo = await _repository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(grupoId));
        if (grupo is null) return false;
        
        var usuarioPassageiro = grupo.Passageiros.Any(p => p.UsuarioId == usuarioId) || grupo?.Motorista.Id == usuarioId;
        return usuarioPassageiro;
    }
    
    public async Task<bool> VerificarGrupoAtingiuLimiteUsuarios(int grupoId)
    {
        var grupo = await _repository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(grupoId));
        return grupo != null && grupo.Passageiros.Count >= grupo.PassageiroLimite;
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
