using FluentAssertions;
using Moq;
using Unirota.Application.Commands.Corridas;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Corrida;
using Unirota.Application.Services;
using Unirota.Application.Services.Corrida;
using Unirota.Application.Specifications.Grupos;
using Unirota.Domain.Entities.Corridas;
using Unirota.Domain.Entities.Grupos;
using Xunit;

namespace Unirota.UnitTests.Application.Handlers;

public class CorridaRequestHandlerTests
{
    private readonly Mock<IReadRepository<Corrida>> _readCorridaRepository = new();
    private readonly Mock<IReadRepository<Grupo>> _readGrupoRepository = new();
    private readonly Mock<ICorridaService> _service = new();
    private readonly Mock<ICurrentUser> _currentUser = new();
    private readonly Mock<IServiceContext> _serviceContext = new();
    private readonly CorridaRequestHandler _handler;

    public CorridaRequestHandlerTests()
    {
        _handler = new(_serviceContext.Object,
                       _currentUser.Object,
                       _service.Object,
                       _readCorridaRepository.Object,
                       _readGrupoRepository.Object);
    }

    [Fact(DisplayName = "Deve retornar o valor padrão quando o grupo não for encontrado")]
    public async Task DeveRetornarValorPadrao_QuandoGrupoNaoForEncontrado()
    {
        // Arrange
        _readGrupoRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Grupo);

        // Act
        var result = await _handler.Handle(new CriarCorridaCommand { GrupoId = 1 }, CancellationToken.None);

        // Assert
        result.Should().Be(default(int));
        _serviceContext.Verify(context => context.AddError("Grupo não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar o valor padrão quando o usuário não for o motorista")]
    public async Task DeveRetornarValorPadrao_QuandoUsuarioNaoForMotorista()
    {
        // Arrange
        var grupo = new Grupo { MotoristaId = 2 };
        _readGrupoRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(grupo);

        _currentUser
            .Setup(user => user.GetUserId())
            .Returns(1);

        // Act
        var result = await _handler.Handle(new CriarCorridaCommand { GrupoId = 1 }, CancellationToken.None);

        // Assert
        result.Should().Be(default);
        _serviceContext.Verify(context => context.AddError("Você não está cadastrado como motorista"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar o ID da corrida quando a criação for bem-sucedida")]
    public async Task DeveRetornarIdCorrida_QuandoCriacaoBemSucedida()
    {
        // Arrange
        var grupo = new Grupo { MotoristaId = 1 };
        _readGrupoRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(grupo);

        _currentUser
            .Setup(user => user.GetUserId())
            .Returns(1);

        _service
            .Setup(service => service.Criar(It.IsAny<CriarCorridaCommand>()))
            .ReturnsAsync(42);

        // Act
        var result = await _handler.Handle(new CriarCorridaCommand { GrupoId = 1 }, CancellationToken.None);

        // Assert
        result.Should().Be(42);
    }

    [Fact(DisplayName = "Deve retornar a lista de corridas quando a consulta for bem-sucedida")]
    public async Task DeveRetornarListaCorridas_QuandoConsultaBemSucedida()
    {
        // Arrange
        var expectedCorridas = new List<Corrida>
        {
            new() { Comeco = DateTime.Now },
            new() { Comeco = DateTime.Now.AddDays(1) }
        };

        _service
            .Setup(x => x.ObterPorIdDeGrupo(It.IsAny<ConsultarCorridaPorIdQuery>(), CancellationToken.None))
            .ReturnsAsync(expectedCorridas);

        // Act
        var result = await _handler.Handle(new ConsultarCorridaPorIdQuery { Id = 1 }, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedCorridas);
    }

    [Fact(DisplayName = "Deve retornar uma lista vazia quando nenhuma corrida for encontrada")]
    public async Task DeveRetornarListaVazia_QuandoNenhumaCorridaEncontrada()
    {
        // Arrange
        var expectedCorridas = new List<Corrida>();
        _service
            .Setup(service => service.ObterPorIdDeGrupo(It.IsAny<ConsultarCorridaPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCorridas);

        // Act
        var result = await _handler.Handle(new ConsultarCorridaPorIdQuery { Id = 1 }, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
