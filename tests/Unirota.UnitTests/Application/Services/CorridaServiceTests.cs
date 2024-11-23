using FluentAssertions;
using Moq;
using Unirota.Application.Commands.Corridas;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Corrida;
using Unirota.Application.Services;
using Unirota.Application.Services.Corrida;
using Unirota.Application.Specification.Corridas;
using Unirota.Domain.Entities.Corridas;
using Xunit;

namespace Unirota.UnitTests.Application.Services;

public class CorridaServiceTests
{
    private readonly Mock<IRepository<Corrida>> _repository = new();
    private readonly Mock<IReadRepository<Corrida>> _readRepository = new();
    private readonly Mock<IServiceContext> _serviceContext = new();
    private readonly CorridaService _service;

    public CorridaServiceTests()
    {
        _service = new(_repository.Object,
                       _serviceContext.Object,
                       _readRepository.Object);
    }

    [Fact(DisplayName = "Deve criar uma nova corrida e retornar o ID da corrida")]
    public async Task DeveCriarNovaCorridaERetornarId()
    {
        // Arrange
        var corrida = new Corrida { Comeco = DateTime.Now };
        _repository
            .Setup(repo => repo.AddAsync(It.IsAny<Corrida>(), CancellationToken.None))
            .ReturnsAsync(corrida);

        // Act
        var result = await _service.Criar(new CriarCorridaCommand { GrupoId = 1 });

        // Assert
        result.Should().NotBe(null);
        _repository.Verify(repo => repo.AddAsync(It.IsAny<Corrida>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar lista de corridas para o grupo especificado")]
    public async Task DeveRetornarListaDeCorridasParaGrupoEspecificado()
    {
        // Arrange
        var expectedCorridas = new List<Corrida>
        {
            new() { Comeco = DateTime.Now },
            new() { Comeco = DateTime.Now }
        };
        _readRepository
            .Setup(repo => repo.ListAsync(It.IsAny<ConsultarCorridaPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCorridas);

        // Act
        var result = await _service.ObterPorIdDeGrupo(new ConsultarCorridaPorIdQuery { Id = 1 }, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedCorridas);
    }

    [Fact(DisplayName = "Deve adicionar erro ao contexto de serviço quando não houver corridas para o grupo")]
    public async Task DeveAdicionarErroQuandoNaoExistirCorridasParaGrupo()
    {
        // Arrange
        _readRepository
            .Setup(repo => repo.ListAsync(It.IsAny<ConsultarCorridaPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<Corrida>)null);

        // Act
        var result = await _service.ObterPorIdDeGrupo(new ConsultarCorridaPorIdQuery { Id = 1 }, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _serviceContext.Verify(context => context.AddError("Não existe corridas para este grupo"), Times.Once);
    }
}
