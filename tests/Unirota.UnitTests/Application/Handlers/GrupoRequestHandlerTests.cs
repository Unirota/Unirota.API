using FluentAssertions;
using Moq;
using System.ComponentModel;
using Unirota.Application.Commands.Grupos;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Grupo;
using Unirota.Application.Services;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Specifications.Grupos;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Application.ViewModels.Grupos;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Usuarios;
using Unirota.UnitTests.Builder;
using Xunit;

namespace Unirota.UnitTests.Application.Handlers;

public class GrupoRequestHandlerTests
{
    private readonly Mock<ICurrentUser> _currentUser = new();
    private readonly Mock<IReadRepository<Usuario>> _readUserRepository = new();
    private readonly Mock<IReadRepository<Grupo>> _readGrupoRepository = new();
    private readonly Mock<IGrupoService> _service = new();
    private readonly Mock<IServiceContext> _serviceContext = new();
    private readonly GrupoRequestHandler _handler;

    public GrupoRequestHandlerTests()
    {
        _handler = new(_serviceContext.Object,
                       _currentUser.Object,
                       _service.Object,
                       _readUserRepository.Object,
                       _readGrupoRepository.Object);
    }

    [Fact(DisplayName = "Deve retornar o valor padrão quando o motorista não é encontrado")]
    public async Task Handle_ShouldReturnDefault_WhenMotoristaIsNull()
    {
        // Arrange
        _currentUser.Setup(x => x.GetUserId()).Returns(1);
        _readUserRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Usuario);

        // Act
        var result = await _handler.Handle(new CriarGrupoCommand(), CancellationToken.None);

        // Assert
        result.Should().Be(0);
        _serviceContext.Verify(x => x.AddError("Motorista não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar o valor padrão quando o motorista não possui habilitação cadastrada")]
    public async Task Handle_ShouldReturnDefault_WhenMotoristaHasNoHabilitacao()
    {
        // Arrange
        var motorista = new UsuarioBuilder().Build();
        _currentUser.Setup(x => x.GetUserId()).Returns(motorista.Id);
        _readUserRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(motorista);

        // Act
        var result = await _handler.Handle(new CriarGrupoCommand(), CancellationToken.None);

        // Assert
        result.Should().Be(0);
        _serviceContext.Verify(x => x.AddError("Motorista informado não possui habilitação cadastrada"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar o ID do grupo quando a criação é bem-sucedida")]
    public async Task Handle_ShouldReturnGrupoId_WhenCreationIsSuccessful()
    {
        // Arrange
        var motorista = new UsuarioBuilder().WithHabilitacao("11111111111").Build();
        var expectedGrupoId = 1;
        _currentUser.Setup(x => x.GetUserId()).Returns(motorista.Id);
        _readUserRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(motorista);
        _service.Setup(x => x.Criar(It.IsAny<CriarGrupoCommand>(), motorista.Id))
            .ReturnsAsync(expectedGrupoId);

        // Act
        var result = await _handler.Handle(new CriarGrupoCommand(), CancellationToken.None);

        // Assert
        result.Should().Be(expectedGrupoId);
    }

    [Fact(DisplayName = "Deve retornar o grupo quando o grupo é encontrado pelo ID")]
    public async Task Handle_ShouldReturnGrupo_WhenGrupoIsFound()
    {
        // Arrange
        var expectedGrupo = new GrupoBuilder()
            .WithNome("Grupo Teste")
            .WithLimite(15)
            .WithInicio(DateTime.Now)
            .WithMotoristaId(123)
            .WithDestino("Destino Teste")
            .Build();

        var request = new ConsultarGrupoPorIdQuery { Id = expectedGrupo.Id };

        _service
            .Setup(x => x.ObterPorId(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedGrupo);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedGrupo);
        _service.Verify(x => x.ObterPorId(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [DisplayName("Deve retornar null quando o grupo não é encontrado pelo ID")]
    public async Task Handle_ShouldReturnNull_WhenGrupoIsNotFound()
    {
        // Arrange
        var request = new ConsultarGrupoPorIdQuery { Id = 999 };

        _service
            .Setup(x => x.ObterPorId(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Grupo);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _service.Verify(x => x.ObterPorId(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar false e adicionar erro ao contexto quando o motorista não é encontrado")]
    public async Task Handle_ShouldReturnFalse_WhenMotoristaNotFound()
    {
        // Arrange
        var command = new DeletarGrupoCommand { Id = 1 };
        _currentUser.Setup(c => c.GetUserId()).Returns(123);
        _readUserRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _serviceContext.Verify(c => c.AddError("Motorista não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar false e adicionar erro ao contexto quando o grupo não é encontrado")]
    public async Task Handle_ShouldReturnFalse_WhenGrupoNotFound()
    {
        // Arrange
        var command = new DeletarGrupoCommand { Id = 1 };
        var motorista = new UsuarioBuilder().Build();

        _currentUser.Setup(c => c.GetUserId()).Returns(motorista.Id);
        _readUserRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(motorista);
        _readGrupoRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Grupo);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _serviceContext.Verify(c => c.AddError("Grupo não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar false e adicionar erro ao contexto quando o usuário não é o motorista do grupo")]
    public async Task Handle_ShouldReturnFalse_WhenUserIsNotMotorista()
    {
        // Arrange
        var command = new DeletarGrupoCommand { Id = 1 };
        var motorista = new UsuarioBuilder().Build();
        var grupo = new GrupoBuilder()
            .WithMotoristaId(456)  // ID diferente do motorista
            .Build();

        _currentUser.Setup(c => c.GetUserId()).Returns(motorista.Id);
        _readUserRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(motorista);
        _readGrupoRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(grupo);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _serviceContext.Verify(c => c.AddError("O usuário está tentando apagar um grupo que não é motorista"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar true e deletar o grupo quando o usuário é o motorista do grupo")]
    public async Task Handle_ShouldReturnTrue_WhenUserIsMotoristaOfGrupo()
    {
        // Arrange
        var command = new DeletarGrupoCommand { Id = 1 };
        var motorista = new UsuarioBuilder().Build();
        var grupo = new GrupoBuilder()
            .WithMotoristaId(motorista.Id)
            .Build();

        _currentUser.Setup(c => c.GetUserId()).Returns(motorista.Id);
        _readUserRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(motorista);
        _readGrupoRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(grupo);
        _service.Setup(s => s.Deletar(command, grupo)).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _service.Verify(s => s.Deletar(command, grupo), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar lista vazia e adicionar erro ao contexto quando o usuário não é encontrado")]
    public async Task Handle_ShouldReturnEmptyList_WhenUsuarioNotFound()
    {
        // Arrange
        var command = new ObterGrupoUsuarioCommand { Id = 1 };
        _currentUser.Setup(c => c.GetUserId()).Returns(1);
        _readUserRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
        _serviceContext.Verify(c => c.AddError("Usuário não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar lista vazia e adicionar erro ao contexto quando o usuário não tem grupos")]
    public async Task Handle_ShouldReturnEmptyList_WhenUsuarioHasNoGrupos()
    {
        // Arrange
        var usuario = new UsuarioBuilder().Build();
        var command = new ObterGrupoUsuarioCommand { Id = usuario.Id };
        

        _currentUser.Setup(c => c.GetUserId()).Returns(usuario.Id);
        _readUserRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
        _serviceContext.Verify(c => c.AddError("Este usuário não tem grupos"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar lista vazia e adicionar erro ao contexto quando o usuário tenta acessar grupos de outro usuário")]
    public async Task Handle_ShouldReturnEmptyList_WhenUserTriesToAccessOtherUserGroups()
    {
        // Arrange
        var usuario = new UsuarioBuilder().Build();
        var command = new ObterGrupoUsuarioCommand { Id = 13 };

        _currentUser.Setup(c => c.GetUserId()).Returns(usuario.Id);
        _readUserRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
        _serviceContext.Verify(c => c.AddError("Usuário não pode consultar grupos de outro usuário"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar lista de grupos do usuário quando o usuário é válido e tem grupos")]
    public async Task Handle_ShouldReturnGruposList_WhenUserIsValidAndHasGrupos()
    {
        // Arrange
        var command = new ObterGrupoUsuarioCommand { Id = 0 };
        var usuario = new UsuarioBuilder().Build();
        usuario.AdicionarGrupo(new GrupoBuilder().Build());

        var grupos = new List<ListarGruposViewModel>
        {
            new ListarGruposViewModel { Id = 1, Nome = "Grupo 1" }
        };

        _currentUser.Setup(c => c.GetUserId()).Returns(usuario.Id);
        _readUserRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);
        _service
            .Setup(service => service.ObterPorUsuarioId(command.Id))
            .ReturnsAsync(grupos);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
        result.First().Nome.Should().Be("Grupo 1");
    }
}
