using Unirota.Application.Commands.Grupos;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Grupo;
using Unirota.Application.Specification.Grupo;
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

    public Task<int> Editar(EditarGrupoCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Grupo?> ObterPorId(ConsultarGrupoPorIdQuery request, CancellationToken cancellationToken)
    {
        Grupo? grupo = await _repository.FirstOrDefaultAsync(new ConsultarGrupoPorIdSpec(request.Id), cancellationToken);
        var teste = await _repository.ListAsync();
        if(grupo == null)
            _serviceContext.AddError("Grupo não encontrado");
        return grupo;
    }
}
