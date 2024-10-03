using Unirota.Application.Commands.Veiculos;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Persistence;
using Unirota.Application.Specification.Veiculos;
using Unirota.Domain.Entities.Veiculos;

namespace Unirota.Application.Services.Veiculos
{
    public class VeiculoService : IVeiculoService
    {
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Veiculo> _repository;
        private readonly IServiceContext _serviceContext;

        public VeiculoService(ICurrentUser currentUser, IRepository<Veiculo> repository, IServiceContext serviceContext) 
        {
            _currentUser = currentUser;
            _repository = repository;
            _serviceContext = serviceContext;
        }

        public async Task<int> Criar(CriarVeiculosCommand request, CancellationToken cancellationToken)
        {
            Veiculo? veiculo = await _repository.FirstOrDefaultAsync(new ConsultarVeiculoPorPlacaSpec(request.Placa), cancellationToken);
            if (veiculo is not null)
            {
                _serviceContext.AddError("Veículo já cadastrado");
                return default;
            }

            int currentUserId = _currentUser.GetUserId();
            Veiculo novoVeiculo = new Veiculo(request.Placa, currentUserId, request.Cor, request.Carroceria, request.Descricao);
            await _repository.AddAsync(novoVeiculo);
            return novoVeiculo.Id;
        }
    }
}
