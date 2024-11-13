using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unirota.Application.Persistence;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Services;
using Unirota.Domain.Entities.Mensagens;
using Moq;
using Unirota.Application.Services.Mensagens;
using Unirota.Application.Commands.Mensagens;
using Unirota.Application.Queries.Grupo;
using Unirota.Domain.Entities.Grupos;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Unirota.Application.Hubs;

namespace Unirota.UnitTests.Application.Services;

public class MensagemServiceTests
{
    private readonly Mock<IRepository<Mensagem>> _mensagemRepository = new();
    private readonly Mock<IGrupoService> _grupoService = new();
    private readonly Mock<IUsuarioService> _usuarioService = new();
    private readonly Mock<IServiceContext> _serviceContext = new();
    private readonly Mock<IHubContext<ChatHub>> _chatHub = new();
    private MensagemService _service;

    public MensagemServiceTests()
    {
        _service = new(_mensagemRepository.Object,
                       _grupoService.Object,
                       _usuarioService.Object,
                       _serviceContext.Object,
                       _chatHub.Object);
    }

    [Fact(DisplayName = "Deve criar mensagem quando todos os dados estiverem corretos")]
    public async Task DeveCriarMensagem_QuandoDadosCorretos()
    {
        // Arrange
        var usuarioId = 123;
        var grupoId = 1;
        var comando = new CriarMensagemCommand { Conteudo = "Teste de mensagem", GrupoId = grupoId };
        var grupo = new Grupo();

        _grupoService.Setup(g => g.ObterPorId(It.IsAny<ConsultarGrupoPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(grupo);
        _usuarioService.Setup(u => u.VerificarUsuarioExiste(usuarioId)).ReturnsAsync(true);
        _grupoService.Setup(g => g.VerificarUsuarioPertenceAoGrupo(usuarioId, grupoId)).ReturnsAsync(true);
        _mensagemRepository.Setup(m => m.AddAsync(It.IsAny<Mensagem>(), CancellationToken.None)).ReturnsAsync(It.IsAny<Mensagem>());

        // Act
        var result = await _service.Criar(comando, usuarioId);

        // Assert
        result.Should().Be(0);
        _serviceContext.Verify(context => context.AddError(It.IsAny<string>()), Times.Never);
    }

    [Fact(DisplayName = "Deve adicionar erro quando grupo não encontrado")]
    public async Task DeveAdicionarErro_QuandoGrupoNaoEncontrado()
    {
        // Arrange
        var usuarioId = 123;
        var grupoId = 1;
        var comando = new CriarMensagemCommand { Conteudo = "Teste de mensagem", GrupoId = grupoId };

        _grupoService.Setup(g => g.ObterPorId(It.IsAny<ConsultarGrupoPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Grupo);

        // Act
        var result = await _service.Criar(comando, usuarioId);

        // Assert
        result.Should().Be(0);
        _serviceContext.Verify(context => context.AddError("Grupo não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve adicionar erro quando usuário não encontrado")]
    public async Task DeveAdicionarErro_QuandoUsuarioNaoEncontrado()
    {
        // Arrange
        var usuarioId = 123;
        var grupoId = 1;
        var comando = new CriarMensagemCommand { Conteudo = "Teste de mensagem", GrupoId = grupoId };
        var grupo = new Grupo();

        _grupoService.Setup(g => g.ObterPorId(It.IsAny<ConsultarGrupoPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(grupo);
        _usuarioService.Setup(u => u.VerificarUsuarioExiste(usuarioId)).ReturnsAsync(false);

        // Act
        var result = await _service.Criar(comando, usuarioId);

        // Assert
        result.Should().Be(0);
        _serviceContext.Verify(context => context.AddError("Usuário não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve adicionar erro quando usuário não pertence ao grupo")]
    public async Task DeveAdicionarErro_QuandoUsuarioNaoPertenceAoGrupo()
    {
        // Arrange
        var usuarioId = 123;
        var grupoId = 1;
        var comando = new CriarMensagemCommand { Conteudo = "Teste de mensagem", GrupoId = grupoId };
        var grupo = new Grupo();

        _grupoService.Setup(g => g.ObterPorId(It.IsAny<ConsultarGrupoPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(grupo);
        _usuarioService.Setup(u => u.VerificarUsuarioExiste(usuarioId)).ReturnsAsync(true);
        _grupoService.Setup(g => g.VerificarUsuarioPertenceAoGrupo(usuarioId, grupoId)).ReturnsAsync(false);

        // Act
        var result = await _service.Criar(comando, usuarioId);

        // Assert
        result.Should().Be(0);
        _serviceContext.Verify(context => context.AddError("Usuário não pertence ao grupo"), Times.Once);
    }

    [Fact(DisplayName = "Deve adicionar erro quando ocorre uma exceção ao criar a mensagem")]
    public async Task DeveAdicionarErro_QuandoOcorrerExcecaoAoCriarMensagem()
    {
        // Arrange
        var usuarioId = 123;
        var grupoId = 1;
        var comando = new CriarMensagemCommand { Conteudo = "Teste de mensagem", GrupoId = grupoId };
        var grupo = new Grupo();

        _grupoService.Setup(g => g.ObterPorId(It.IsAny<ConsultarGrupoPorIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(grupo);
        _usuarioService.Setup(u => u.VerificarUsuarioExiste(usuarioId)).ReturnsAsync(true);
        _grupoService.Setup(g => g.VerificarUsuarioPertenceAoGrupo(usuarioId, grupoId)).ReturnsAsync(true);
        _mensagemRepository.Setup(m => m.AddAsync(It.IsAny<Mensagem>(), CancellationToken.None)).ThrowsAsync(new ArgumentException("Erro ao criar mensagem"));

        // Act
        var result = await _service.Criar(comando, usuarioId);

        // Assert
        result.Should().Be(0);
        _serviceContext.Verify(context => context.AddError("Erro ao criar mensagem"), Times.Once);
    }
}
