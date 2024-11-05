using Moq;
using Unirota.Application.Commands.Convites;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers;
using Unirota.Application.Persistence;
using Unirota.Application.Services;
using Unirota.Application.Services.Convites;
using Unirota.Application.Specifications.Convites;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Domain.Entities.Covites;
using Unirota.Domain.Entities.Usuarios;
using Unirota.UnitTests.Builder;
using Xunit;

namespace Unirota.UnitTests.Application.Handlers;

public class ConviteRequestHandlerTests
{
    private readonly Mock<IRepository<Convite>> _repository = new();
    private readonly Mock<IReadRepository<Convite>> _readRepository = new();
    private readonly Mock<IReadRepository<Usuario>> _readUserRepository = new();
    private readonly Mock<ICurrentUser> _currentUser = new();
    private readonly Mock<IServiceContext> _serviceContext = new();
    private readonly Mock<IConviteService> _service = new();
    private readonly ConviteRequestHandler _handler;

    public ConviteRequestHandlerTests()
    {
        _handler = new(_serviceContext.Object,
                       _repository.Object,
                       _readRepository.Object,
                       _readUserRepository.Object,
                       _currentUser.Object,
                       _service.Object);
    }

    [Fact(DisplayName = "Deve retornar o convite criado com sucesso")]
    public async Task Handle_DeveRetornarConvite_QuandoMotoristaEHabilitadoEUsuarioEncontrado()
    {
        // Arrange
        var request = new CriarConviteCommand { MotoristaId = 1, UsuarioId = 2, GrupoId = 3 };
        var motorista = new UsuarioBuilder().WithHabilitacao("11111111111").Build();
        _readUserRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(motorista);
        _readUserRepository.Setup(repo => repo.AnyAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _service.Setup(s => s.Criar(It.IsAny<CriarConviteCommand>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(1, result); 
        _serviceContext.Verify(sc => sc.AddError(It.IsAny<string>()), Times.Never);
    }

    [Fact(DisplayName = "Deve adicionar erro e retornar 0 quando motorista não encontrado")]
    public async Task Handle_DeveRetornarZero_QuandoMotoristaNaoEncontrado()
    {
        // Arrange
        var request = new CriarConviteCommand { MotoristaId = 1, UsuarioId = 2, GrupoId = 3 };
        _readUserRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Usuario);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(default, result);
        _serviceContext.Verify(sc => sc.AddError("Motorista não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve adicionar erro e retornar 0 quando motorista não possui habilitação")]
    public async Task Handle_DeveRetornarZero_QuandoMotoristaSemHabilitacao()
    {
        // Arrange
        var request = new CriarConviteCommand { MotoristaId = 1, UsuarioId = 2, GrupoId = 3 };
        var motorista = new UsuarioBuilder().Build();
        _readUserRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(motorista);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(default, result);
        _serviceContext.Verify(sc => sc.AddError("Motorista informado não possui habilitação cadastrada"), Times.Once);
    }

    [Fact(DisplayName = "Deve adicionar erro e retornar 0 quando usuário não encontrado")]
    public async Task Handle_DeveRetornarZero_QuandoUsuarioNaoEncontrado()
    {
        // Arrange
        var request = new CriarConviteCommand { MotoristaId = 1, UsuarioId = 2, GrupoId = 3 };
        var motorista = new UsuarioBuilder().WithHabilitacao("11111111111").Build();
        _readUserRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(motorista);
        _readUserRepository.Setup(repo => repo.AnyAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(default, result);
        _serviceContext.Verify(sc => sc.AddError("Usuário não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve cancelar o convite com sucesso")]
    public async Task Handle_DeveCancelarConvite_QuandoConviteExistenteENaoAceito()
    {
        // Arrange
        var request = new CancelarConvitePorIdCommand { Id = 1 };
        var convite = new Convite(1, 1, 1); 

        convite.Aceito = false;
        _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarConvitePorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(convite);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _service.Verify(s => s.Cancelar(convite), Times.Once);
        _serviceContext.Verify(sc => sc.AddError(It.IsAny<string>()), Times.Never);
    }

    [Fact(DisplayName = "Deve adicionar erro e retornar false quando convite não encontrado")]
    public async Task Handle_DeveRetornarFalse_QuandoConviteNaoEncontradoAoCancelar()
    {
        // Arrange
        var request = new CancelarConvitePorIdCommand { Id = 1 };
        _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarConvitePorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Convite);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("Convite não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve adicionar erro e retornar false quando convite já aceito")]
    public async Task Handle_DeveRetornarFalse_QuandoConviteAceito()
    {
        // Arrange
        var request = new CancelarConvitePorIdCommand { Id = 1 };
        var convite = new Convite(1, 1, 1);
        
        convite.Aceito = true;
        _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarConvitePorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(convite);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("Não é possível cancelar um convite que já foi aceito"), Times.Once);
        _service.Verify(s => s.Cancelar(It.IsAny<Convite>()), Times.Never);
    }

    [Fact(DisplayName = "Deve aceitar o convite com sucesso")]
    public async Task Handle_DeveAceitarConvite_QuandoConviteExistente()
    {
        // Arrange
        var request = new AceitarConviteCommand { Id = 1 };
        _service.Setup(s => s.Aceitar(It.IsAny<AceitarConviteCommand>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _service.Verify(s => s.Aceitar(request), Times.Once);
        _serviceContext.Verify(sc => sc.AddError(It.IsAny<string>()), Times.Never);
    }

    [Fact(DisplayName = "Deve retornar false e adicionar erro quando convite não encontrado")]
    public async Task Handle_DeveRetornarFalse_QuandoConviteNaoEncontradoAoAceitar()
    {
        // Arrange
        var request = new AceitarConviteCommand { Id = 1 };
        _service.Setup(s => s.Aceitar(It.IsAny<AceitarConviteCommand>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _service.Verify(s => s.Aceitar(request), Times.Once);
    }

    [Fact(DisplayName = "Deve recusar o convite com sucesso")]
    public async Task Handle_DeveRecusarConvite_QuandoConviteExistenteEValido()
    {
        // Arrange
        var userId = 1;
        var request = new RecusarConviteCommand { Id = 1 };
        var convite = new Convite(userId, 2, 3);

        _currentUser.Setup(u => u.GetUserId()).Returns(userId);
        _repository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<ConsultarConvitePorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(convite);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _service.Verify(s => s.Cancelar(convite), Times.Once);
        _serviceContext.Verify(sc => sc.AddError(It.IsAny<string>()), Times.Never);
    }

    [Fact(DisplayName = "Deve retornar false e adicionar erro quando convite não encontrado")]
    public async Task Handle_DeveRetornarFalse_QuandoConviteNaoEncontrado()
    {
        // Arrange
        var request = new RecusarConviteCommand { Id = 1 };
        _currentUser.Setup(u => u.GetUserId()).Returns(1);
        _repository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<ConsultarConvitePorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Convite?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _service.Verify(s => s.Cancelar(It.IsAny<Convite>()), Times.Never);
        _serviceContext.Verify(sc => sc.AddError("Convite não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar false e adicionar erro quando convite já foi aceito")]
    public async Task Handle_DeveRetornarFalse_QuandoConviteJaAceito()
    {
        // Arrange
        var request = new RecusarConviteCommand { Id = 1 };
        var convite = new Convite(1, 2, 3) { Aceito = true };

        _currentUser.Setup(u => u.GetUserId()).Returns(1);
        _repository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<ConsultarConvitePorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(convite);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _service.Verify(s => s.Cancelar(It.IsAny<Convite>()), Times.Never);
        _serviceContext.Verify(sc => sc.AddError("Não é possível recusar um convite que já foi aceito"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar false e adicionar erro quando convite não é direcionado ao usuário")]
    public async Task Handle_DeveRetornarFalse_QuandoConviteNaoDirecionadoAoUsuario()
    {
        // Arrange
        var request = new RecusarConviteCommand { Id = 1 };
        var convite = new Convite(2, 2, 3);

        _currentUser.Setup(u => u.GetUserId()).Returns(1);
        _repository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<ConsultarConvitePorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(convite);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _service.Verify(s => s.Cancelar(It.IsAny<Convite>()), Times.Never);
        _serviceContext.Verify(sc => sc.AddError("Não é possível recusar um convite não direcionado a você"), Times.Once);
    }
}
