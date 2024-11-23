using FluentAssertions;
using Moq;
using Unirota.Application.Commands.Veiculos;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Persistence;
using Unirota.Application.Services;
using Unirota.Application.Services.Veiculos;
using Unirota.Application.Specifications.Veiculos;
using Unirota.Domain.Entities.Veiculos;
using Xunit;

namespace Unirota.UnitTests.Application.Services;

public class VeiculoServiceTests
{
    private readonly Mock<ICurrentUser> _currentUser = new();
    private readonly Mock<IRepository<Veiculo>> _repository = new();
    private readonly Mock<IServiceContext> _serviceContext = new();
    private VeiculoService _service;

    public VeiculoServiceTests()
    {
        _service = new(_currentUser.Object,
                       _repository.Object,
                       _serviceContext.Object);
    }

    [Fact(DisplayName = "Deve criar um veículo quando o veículo não está cadastrado")]
    public async Task DeveCriarVeiculo_QuandoVeiculoNaoEstaCadastrado()
    {
        // Arrange
        var request = new CriarVeiculosCommand
        {
            Placa = "ABC-1234",
            Cor = "Vermelho",
            Carroceria = "Sedan",
            Descricao = "Um carro esportivo"
        };
        _currentUser.Setup(u => u.GetUserId()).Returns(1);
        _repository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<ConsultarVeiculoPorPlacaSpec>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(null as Veiculo);

        // Act
        int result = await _service.Criar(request, CancellationToken.None);

        // Assert
        result.Should().Be(0);
        _repository.Verify(r => r.AddAsync(It.Is<Veiculo>(v => v.Placa == request.Placa &&
                                                               v.Cor == request.Cor &&
                                                               v.Carroceria == request.Carroceria &&
                                                               v.Descricao == request.Descricao), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar 0 e adicionar erro quando o veículo já está cadastrado")]
    public async Task DeveRetornarZero_EAdicionarErro_QuandoVeiculoJaEstaCadastrado()
    {
        // Arrange
        var request = new CriarVeiculosCommand
        {
            Placa = "ABC-1234",
            Cor = "Vermelho",
            Carroceria = "Sedan",
            Descricao = "Um carro esportivo"
        };
        var veiculoExistente = new Veiculo(request.Placa, 1, request.Cor, request.Carroceria, request.Descricao);
        _currentUser.Setup(u => u.GetUserId()).Returns(1);
        _repository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<ConsultarVeiculoPorPlacaSpec>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(veiculoExistente);

        // Act
        int result = await _service.Criar(request, CancellationToken.None);

        // Assert
        result.Should().Be(0);
        _serviceContext.Verify(s => s.AddError("Veículo já cadastrado"), Times.Once);
        _repository.Verify(r => r.AddAsync(It.IsAny<Veiculo>(), CancellationToken.None), Times.Never);
    }
}
