using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unirota.Application.Persistence;
using Unirota.Application.Services.Corrida;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Services;
using Unirota.Domain.Entities.Avaliacoes;
using Moq;
using Unirota.Application.Services.Avaliacoes;
using Unirota.Application.Commands.Avaliacoes;
using Unirota.Application.Queries.Corrida;
using Unirota.Domain.Entities.Corridas;
using Xunit;
using FluentAssertions;
using Unirota.Application.Specifications.Avaliacoes;
using Unirota.Application.ViewModels.Avaliacoes;

namespace Unirota.UnitTests.Application.Services;

public class AvaliacaoServiceTests
{
    private readonly Mock<IRepository<Avaliacao>> _avaliacaoRepository = new();
    private readonly Mock<ICorridaService> corridaService = new();
    private readonly Mock<IUsuarioService> usuarioService = new();
    private readonly Mock<IServiceContext> serviceContext = new();
    private readonly AvaliacaoService _service;

    public AvaliacaoServiceTests()
    {
        _service = new(_avaliacaoRepository.Object,
                       corridaService.Object,
                       usuarioService.Object,
                       serviceContext.Object);
    }

    [Fact(DisplayName = "Deve criar avaliação e retornar o ID da avaliação quando a corrida e o usuário existem")]
    public async Task DeveCriarAvaliacaoERetornarId_QuandoCorridaEUsuarioExistem()
    {
        // Arrange
        var command = new CriarAvaliacaoCommand { CorridaId = 1, Nota = 5 };

        corridaService
            .Setup(s => s.ObterPorIdDeGrupo(It.IsAny<ConsultarCorridaPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Corrida> { new () });

        usuarioService
            .Setup(s => s.VerificarUsuarioExiste(It.IsAny<int>()))
            .ReturnsAsync(true);

        _avaliacaoRepository
            .Setup(repo => repo.AddAsync(It.IsAny<Avaliacao>(), CancellationToken.None))
            .ReturnsAsync(It.IsAny<Avaliacao>());

        // Act
        var result = await _service.Criar(command, 123);

        // Assert
        result.Should().Be(0);
    }

    [Fact(DisplayName = "Deve adicionar erro quando a corrida não for encontrada")]
    public async Task DeveAdicionarErro_QuandoCorridaNaoEncontrada()
    {
        // Arrange
        var command = new CriarAvaliacaoCommand { CorridaId = 1 };

        corridaService
            .Setup(s => s.ObterPorIdDeGrupo(It.IsAny<ConsultarCorridaPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Corrida>());

        // Act
        var result = await _service.Criar(command, 123);

        // Assert
        result.Should().Be(0);
        serviceContext.Verify(context => context.AddError("Corrida não encontrada"), Times.Once);
    }

    [Fact(DisplayName = "Deve adicionar erro quando o usuário não for encontrado")]
    public async Task DeveAdicionarErro_QuandoUsuarioNaoEncontrado()
    {
        // Arrange
        var command = new CriarAvaliacaoCommand { CorridaId = 0 };

        corridaService
            .Setup(s => s.ObterPorIdDeGrupo(It.IsAny<ConsultarCorridaPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Corrida> { new () });

        usuarioService
            .Setup(s => s.VerificarUsuarioExiste(It.IsAny<int>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.Criar(command, 123);

        // Assert
        result.Should().Be(0);
        serviceContext.Verify(context => context.AddError("Usuário não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve adicionar erro quando ocorrer exceção ao criar avaliação")]
    public async Task DeveAdicionarErro_QuandoExcecaoAoCriarAvaliacao()
    {
        // Arrange
        var command = new CriarAvaliacaoCommand { CorridaId = 0, Nota = 5 };

        corridaService
            .Setup(s => s.ObterPorIdDeGrupo(It.IsAny<ConsultarCorridaPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Corrida> { new () });

        usuarioService
            .Setup(s => s.VerificarUsuarioExiste(It.IsAny<int>()))
            .ReturnsAsync(true);

        _avaliacaoRepository
            .Setup(repo => repo.AddAsync(It.IsAny<Avaliacao>(), CancellationToken.None))
            .ThrowsAsync(new ArgumentException("Invalid data"));

        // Act
        var result = await _service.Criar(command, 123);

        // Assert
        result.Should().Be(0);
        serviceContext.Verify(context => context.AddError("Invalid data"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar a lista de avaliações ao buscar por ID de corrida")]
    public async Task DeveRetornarAvaliacoes_QuandoCorridaIdValido()
    {
        // Arrange
        int corridaId = 1;
        var avaliacoes = new List<Avaliacao>
        {
            new (),
            new ()
        };

        _avaliacaoRepository
            .Setup(repo => repo.ListAsync(It.IsAny<ConsultarAvaliacoesPorCorridaSpec>(), CancellationToken.None))
            .ReturnsAsync(avaliacoes);

        // Act
        var result = await _service.ObterPorCorridaId(corridaId);

        // Assert
        result.Should().NotBeNull().And.BeOfType<List<ListarAvaliacoesViewModel>>();
        result.Should().HaveCount(2);
        result.Should().AllBeOfType<ListarAvaliacoesViewModel>();
    }
}
