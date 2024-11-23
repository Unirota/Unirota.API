using Microsoft.AspNetCore.SignalR;
using Moq;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Hubs;
using Unirota.Application.Persistence;
using Unirota.Application.Services;
using Unirota.Application.Services.SolicitacaoEntrada;
using Unirota.Application.Specifications.SolicitacaoEntrada;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;
using Unirota.UnitTests.Builder;
using Xunit;

namespace Unirota.UnitTests.Application.Services;

public class SolicitacaoEntradaServiceTests
{
    private readonly Mock<IRepository<SolicitacaoDeEntrada>> _solicitacaoRepository = new();
    private readonly Mock<IServiceContext> _serviceContext = new();
    private readonly Mock<ICurrentUser> _currentUser = new();
    private readonly Mock<IHubContext<ChatHub>> _chatHub = new();
    private readonly SolicitacaoEntradaService _service;

    public SolicitacaoEntradaServiceTests()
    {
        _service = new(_solicitacaoRepository.Object, _serviceContext.Object, _currentUser.Object, _chatHub.Object);
    }

    [Fact(DisplayName = "Deve criar solicitação de entrada com sucesso")]
    public async Task CriarSolicitacaoEntrada_DeveAdicionarSolicitacaoDeEntrada_QuandoParametrosValidos()
    {
        // Arrange
        int usuarioId = 1;
        int grupoId = 2;

        // Act
        var result = await _service.CriarSolicitacaoEntrada(usuarioId, grupoId);

        // Assert
        Assert.True(result);
        _solicitacaoRepository.Verify(repo => repo.AddAsync(It.Is<SolicitacaoDeEntrada>(
            s => s.UsuarioId == usuarioId && s.GrupoId == grupoId
        ), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar verdadeiro ao criar solicitação de entrada")]
    public async Task CriarSolicitacaoEntrada_DeveRetornarVerdadeiro_QuandoSolicitacaoCriada()
    {
        // Arrange
        int usuarioId = 1;
        int grupoId = 2;

        // Act
        var result = await _service.CriarSolicitacaoEntrada(usuarioId, grupoId);

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Deve aceitar solicitação de entrada com sucesso")]
    public async Task AceitarSolicitacaoEntrada_DeveAceitarSolicitacao_QuandoSolicitacaoEValida()
    {
        // Arrange
        int solicitacaoId = 1;
        int motoristaId = 10;
        var grupo = new GrupoBuilder().WithMotoristaId(motoristaId).Build();
        var solicitacao = new SolicitacaoDeEntrada(2, grupo.Id);
        solicitacao.Grupo = grupo;

        _solicitacaoRepository.Setup(repo => repo.FirstOrDefaultAsync(
            It.IsAny<ConsultarSolicitacaoEntradaPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(solicitacao);

        _currentUser.Setup(c => c.GetUserId()).Returns(motoristaId);

        // Act
        var result = await _service.AceitarSolicitacaoEntrada(solicitacaoId, "", CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.Contains(solicitacao.Grupo.Passageiros, p => p.UsuarioId == solicitacao.UsuarioId);
        _solicitacaoRepository.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar falso e adicionar erro quando solicitação não é encontrada")]
    public async Task AceitarSolicitacaoEntrada_DeveRetornarFalso_QuandoSolicitacaoNaoEncontrada()
    {
        // Arrange
        int solicitacaoId = 1;

        _solicitacaoRepository.Setup(repo => repo.FirstOrDefaultAsync(
            It.IsAny<ConsultarSolicitacaoEntradaPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as SolicitacaoDeEntrada);

        // Act
        var result = await _service.AceitarSolicitacaoEntrada(solicitacaoId, "", CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("Solicitação foi excluída ou aceita."), Times.Once);
        _solicitacaoRepository.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Never);
    }

    [Fact(DisplayName = "Deve retornar falso e adicionar erro quando usuário não é o motorista")]
    public async Task AceitarSolicitacaoEntrada_DeveRetornarFalso_QuandoUsuarioNaoEMotorista()
    {
        // Arrange
        int solicitacaoId = 1;
        int motoristaId = 10;
        int usuarioNaoMotoristaId = 5;
        var solicitacao = new SolicitacaoDeEntrada(2, 0);
        var grupo = new GrupoBuilder().WithMotoristaId(motoristaId).Build();
        solicitacao.Grupo = grupo;

        _solicitacaoRepository.Setup(repo => repo.FirstOrDefaultAsync(
            It.IsAny<ConsultarSolicitacaoEntradaPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(solicitacao);

        _currentUser.Setup(c => c.GetUserId()).Returns(usuarioNaoMotoristaId);

        // Act
        var result = await _service.AceitarSolicitacaoEntrada(solicitacaoId, "", CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("Somente o motorista do grupo pode aprovar solicitação de entrada"), Times.Once);
        _solicitacaoRepository.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Never);
    }

    [Fact(DisplayName = "Deve recusar solicitação de entrada com sucesso")]
    public async Task RecusarSolicitacaoEntrada_DeveRecusarSolicitacao_QuandoSolicitacaoEValida()
    {
        // Arrange
        int solicitacaoId = 1;
        int motoristaId = 10;
        var grupo = new GrupoBuilder().WithMotoristaId(motoristaId).Build();
        var solicitacao = new SolicitacaoDeEntrada(2, grupo.Id);
        solicitacao.Grupo = grupo;

        _solicitacaoRepository.Setup(repo => repo.FirstOrDefaultAsync(
            It.IsAny<ConsultarSolicitacaoEntradaPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(solicitacao);

        _currentUser.Setup(c => c.GetUserId()).Returns(motoristaId);

        // Act
        var result = await _service.RecusarSolicitacaoEntrada(solicitacaoId, CancellationToken.None);

        // Assert
        Assert.True(result);
        _solicitacaoRepository.Verify(repo => repo.DeleteAsync(solicitacao, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar falso e adicionar erro quando solicitação não é encontrada")]
    public async Task RecusarSolicitacaoEntrada_DeveRetornarFalso_QuandoSolicitacaoNaoEncontrada()
    {
        // Arrange
        int solicitacaoId = 1;

        _solicitacaoRepository.Setup(repo => repo.FirstOrDefaultAsync(
            It.IsAny<ConsultarSolicitacaoEntradaPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as SolicitacaoDeEntrada);

        // Act
        var result = await _service.RecusarSolicitacaoEntrada(solicitacaoId, CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("Solicitação foi excluída ou aceita."), Times.Once);
        _solicitacaoRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SolicitacaoDeEntrada>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Deve retornar falso e adicionar erro quando usuário não é o motorista")]
    public async Task RecusarSolicitacaoEntrada_DeveRetornarFalso_QuandoUsuarioNaoEMotorista()
    {
        // Arrange
        int solicitacaoId = 1;
        int motoristaId = 10;
        int usuarioNaoMotoristaId = 5;
        var grupo = new GrupoBuilder().WithMotoristaId(motoristaId).Build();
        var solicitacao = new SolicitacaoDeEntrada(2, grupo.Id);
        solicitacao.Grupo = grupo;

        _solicitacaoRepository.Setup(repo => repo.FirstOrDefaultAsync(
            It.IsAny<ConsultarSolicitacaoEntradaPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(solicitacao);

        _currentUser.Setup(c => c.GetUserId()).Returns(usuarioNaoMotoristaId);

        // Act
        var result = await _service.RecusarSolicitacaoEntrada(solicitacaoId, CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("Somente o motorista do grupo pode recusar solicitação de entrada"), Times.Once);
        _solicitacaoRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SolicitacaoDeEntrada>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Deve cancelar solicitação de entrada com sucesso")]
    public async Task CancelarSolicitacaoEntrada_DeveCancelarSolicitacao_QuandoSolicitacaoEValida()
    {
        // Arrange
        int solicitacaoId = 1;
        int usuarioId = 0;
        var solicitacao = new SolicitacaoDeEntrada(usuarioId, 0);
        solicitacao.Usuario = new UsuarioBuilder().Build();

        _solicitacaoRepository.Setup(repo => repo.FirstOrDefaultAsync(
            It.IsAny<ConsultarSolicitacaoEntradaPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(solicitacao);

        _currentUser.Setup(c => c.GetUserId()).Returns(0);

        // Act
        var result = await _service.CancelarSolicitacaoEntrada(solicitacaoId, CancellationToken.None);

        // Assert
        Assert.True(result);
        _solicitacaoRepository.Verify(repo => repo.DeleteAsync(solicitacao, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar falso e adicionar erro quando solicitação não é encontrada")]
    public async Task CancelarSolicitacaoEntrada_DeveRetornarFalso_QuandoSolicitacaoNaoEncontrada()
    {
        // Arrange
        int solicitacaoId = 1;

        _solicitacaoRepository.Setup(repo => repo.FirstOrDefaultAsync(
            It.IsAny<ConsultarSolicitacaoEntradaPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as SolicitacaoDeEntrada);

        // Act
        var result = await _service.CancelarSolicitacaoEntrada(solicitacaoId, CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("Solicitação foi excluída ou aceita."), Times.Once);
        _solicitacaoRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SolicitacaoDeEntrada>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Deve retornar falso e adicionar erro quando usuário não é o criador da solicitação")]
    public async Task CancelarSolicitacaoEntrada_DeveRetornarFalso_QuandoUsuarioNaoECriadorDaSolicitacao()
    {
        // Arrange
        int solicitacaoId = 1;
        int usuarioId = 122;
        int outroUsuarioId = 5;
        var solicitacao = new SolicitacaoDeEntrada(outroUsuarioId, 0);
        solicitacao.Usuario = new UsuarioBuilder().Build();


        _solicitacaoRepository.Setup(repo => repo.FirstOrDefaultAsync(
            It.IsAny<ConsultarSolicitacaoEntradaPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(solicitacao);

        _currentUser.Setup(c => c.GetUserId()).Returns(usuarioId);

        // Act
        var result = await _service.CancelarSolicitacaoEntrada(solicitacaoId, CancellationToken.None);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("Não é possível cancelar solicitação de outro usuário."), Times.Once);
        _solicitacaoRepository.Verify(repo => repo.DeleteAsync(It.IsAny<SolicitacaoDeEntrada>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
