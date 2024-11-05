using Moq;
using Unirota.Application.Commands.SolicitacaoEntrada;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers;
using Unirota.Application.Persistence;
using Unirota.Application.Services;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Services.SolicitacaoEntrada;
using Unirota.Application.Specifications.Grupos;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Usuarios;
using Unirota.UnitTests.Builder;
using Xunit;

namespace Unirota.UnitTests.Application.Handlers;

public class SolicitacaoEntradaRequestHandlerTests
{
    private Mock<IServiceContext> _serviceContext = new();
    private Mock<ICurrentUser> _currentUser = new();
    private Mock<IGrupoService> _grupoService = new();
    private Mock<ISolicitacaoEntradaService> _solicitacaoEntradaService = new();
    private Mock<IReadRepository<Usuario>> _readUserRepository = new();
    private Mock<IReadRepository<Grupo>> _readGrupoRepository = new();
    private SolicitacaoEntradaRequestHandler _handler;

    public SolicitacaoEntradaRequestHandlerTests()
    {
        _handler = new(_serviceContext.Object, _currentUser.Object, _grupoService.Object, _solicitacaoEntradaService.Object, _readUserRepository.Object, _readGrupoRepository.Object);
    }

    [Fact(DisplayName = "Deve retornar falso e adicionar erro quando GrupoId é zero")]
    public async Task Handle_DeveRetornarFalso_QuandoGrupoIdEhZero()
    {
        // Arrange
        var request = new SolicitacaoEntradaGrupoCommand { GrupoId = 0 };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("GrupoId inválido"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar falso e adicionar erro quando Usuário não é encontrado")]
    public async Task Handle_DeveRetornarFalso_QuandoUsuarioNaoEncontrado()
    {
        // Arrange
        var request = new SolicitacaoEntradaGrupoCommand { GrupoId = 1 };
        _currentUser.Setup(cu => cu.GetUserId()).Returns(1);
        _readUserRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Usuario);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("Usuário não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar falso e adicionar erro quando Grupo não é encontrado")]
    public async Task Handle_DeveRetornarFalso_QuandoGrupoNaoEncontrado()
    {
        // Arrange
        var usuario = new UsuarioBuilder().Build();
        var request = new SolicitacaoEntradaGrupoCommand { GrupoId = 1 };
        _currentUser.Setup(cu => cu.GetUserId()).Returns(1);
        _readUserRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);
        _readGrupoRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Grupo);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("Grupo não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar falso e adicionar erro quando Usuário já pertence ao Grupo")]
    public async Task Handle_DeveRetornarFalso_QuandoUsuarioJaPertenceAoGrupo()
    {
        // Arrange
        var grupo = new GrupoBuilder().Build();
        var usuario = new UsuarioBuilder().Build();
        var request = new SolicitacaoEntradaGrupoCommand { GrupoId = 1 };
        _currentUser.Setup(cu => cu.GetUserId()).Returns(usuario.Id);
        _readUserRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);
        _readGrupoRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(grupo);
        _grupoService.Setup(gs => gs.VerificarUsuarioPertenceAoGrupo(usuario.Id, grupo.Id)).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("O usuário já pertence a este grupo"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar falso e adicionar erro quando Grupo atingiu o limite de usuários")]
    public async Task Handle_DeveRetornarFalso_QuandoGrupoAtingiuLimiteUsuarios()
    {
        // Arrange
        var usuario = new UsuarioBuilder().Build();
        var grupo = new GrupoBuilder().Build();
        var request = new SolicitacaoEntradaGrupoCommand { GrupoId = 1 };
        _currentUser.Setup(cu => cu.GetUserId()).Returns(1);
        _readUserRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);
        _readGrupoRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(grupo);
        _grupoService.Setup(gs => gs.VerificarUsuarioPertenceAoGrupo(usuario.Id, grupo.Id)).ReturnsAsync(false);
        _grupoService.Setup(gs => gs.VerificarGrupoAtingiuLimiteUsuarios(grupo.Id)).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("O grupo atingiu o limite de usuários"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar verdadeiro quando solicitação de entrada é criada com sucesso")]
    public async Task Handle_DeveRetornarVerdadeiro_QuandoSolicitacaoCriadaComSucesso()
    {
        // Arrange
        var usuario = new UsuarioBuilder().Build();
        var grupo = new GrupoBuilder().Build();
        var request = new SolicitacaoEntradaGrupoCommand { GrupoId = 1 };
        _currentUser.Setup(cu => cu.GetUserId()).Returns(1);
        _readUserRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);
        _readGrupoRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(grupo);
        _grupoService.Setup(gs => gs.VerificarUsuarioPertenceAoGrupo(usuario.Id, grupo.Id)).ReturnsAsync(false);
        _grupoService.Setup(gs => gs.VerificarGrupoAtingiuLimiteUsuarios(grupo.Id)).ReturnsAsync(false);
        _solicitacaoEntradaService.Setup(se => se.CriarSolicitacaoEntrada(usuario.Id, grupo.Id)).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Deve aceitar solicitação de entrada ao grupo com sucesso")]
    public async Task Handle_DeveAceitarSolicitacaoEntrada_QuandoChamado()
    {
        // Arrange
        var request = new AceitarEntradaGrupoCommand { Id = 1 };
        _solicitacaoEntradaService.Setup(service => service.AceitarSolicitacaoEntrada(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _solicitacaoEntradaService.Verify(service => service.AceitarSolicitacaoEntrada(request.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve recusar solicitação de entrada ao grupo com sucesso")]
    public async Task Handle_DeveRecusarSolicitacaoEntrada_QuandoChamado()
    {
        // Arrange
        var request = new RecusarEntradaGrupoCommand { Id = 1 };
        _solicitacaoEntradaService.Setup(service => service.RecusarSolicitacaoEntrada(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _solicitacaoEntradaService.Verify(service => service.RecusarSolicitacaoEntrada(request.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve cancelar solicitação de entrada ao grupo com sucesso")]
    public async Task Handle_DeveCancelarSolicitacaoEntrada_QuandoChamado()
    {
        // Arrange
        var request = new CancelarSolicitacaoEntradaGrupoCommand { Id = 1 };
        _solicitacaoEntradaService.Setup(service => service.CancelarSolicitacaoEntrada(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _solicitacaoEntradaService.Verify(service => service.CancelarSolicitacaoEntrada(request.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
