using FluentAssertions;
using Moq;
using Unirota.Application.Commands.Mensagens;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Mensagens;
using Unirota.Application.Services;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Services.Mensagens;
using Unirota.Application.Specifications.Mensagens;
using Unirota.Application.ViewModels;
using Unirota.Application.ViewModels.Mensagens;
using Unirota.Domain.Entities.Mensagens;
using Xunit;

namespace Unirota.UnitTests.Application.Handlers;

public class MensagemRequestHandlerTests
{
    private readonly Mock<IMensagemService> _mensagemService = new();
    private readonly Mock<IGrupoService> _grupoService = new();
    private readonly Mock<ICurrentUser> _currentUser = new();
    private readonly Mock<IReadRepository<Mensagem>> _readRepository = new();
    private readonly Mock<IServiceContext> _serviceContext = new();
    private readonly MensagemRequestHandler _handler;

    public MensagemRequestHandlerTests()
    {
        _handler = new(_mensagemService.Object,
                       _grupoService.Object,
                       _currentUser.Object,
                       _readRepository.Object,
                       _serviceContext.Object);
    }

    [Fact(DisplayName = "Deve retornar mensagens quando usuário pertence ao grupo")]
    public async Task DeveRetornarMensagens_QuandoUsuarioPertenceAoGrupo()
    {
        // Arrange
        var request = new ListarMensagensPorGrupoQuery { GrupoId = 1, Pagina = 1, QuantidadeRegistros = 10 };
        var usuarioId = 123;
        var mensagens = new List<Mensagem>
        {
            new Mensagem("Mensagem 1", 0, 0),
            new Mensagem("Mensagem 2", 0, 0),
        };

        _currentUser.Setup(u => u.GetUserId()).Returns(usuarioId);
        _grupoService.Setup(g => g.VerificarUsuarioPertenceAoGrupo(usuarioId, request.GrupoId)).ReturnsAsync(true);
        _readRepository.Setup(r => r.CountAsync(It.IsAny<ListarMensagemBaseSpec>(), It.IsAny<CancellationToken>())).ReturnsAsync(mensagens.Count);
        _readRepository.Setup(r => r.ListAsync(It.IsAny<ListarMensagemBaseSpec>(), It.IsAny<CancellationToken>())).ReturnsAsync(mensagens);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ResultadoPaginadoViewModel<ListarMensagensViewModel>>();
        result.QuantidadeRegistros.Should().Be(mensagens.Count);
    }

    [Fact(DisplayName = "Deve adicionar erro se usuário não pertence ao grupo")]
    public async Task DeveAdicionarErro_QuandoUsuarioNaoPertenceAoGrupo()
    {
        // Arrange
        var request = new ListarMensagensPorGrupoQuery { GrupoId = 1, Pagina = 1, QuantidadeRegistros = 10 };
        var usuarioId = 123;

        _currentUser.Setup(u => u.GetUserId()).Returns(usuarioId);
        _grupoService.Setup(g => g.VerificarUsuarioPertenceAoGrupo(usuarioId, request.GrupoId)).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _serviceContext.Verify(context => context.AddError("Usuário só pode consultar mensagens de um grupo que participa"), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar uma coleção vazia se não houver mensagens")]
    public async Task DeveRetornarColecaoVazia_QuandoSemMensagens()
    {
        // Arrange
        var request = new ListarMensagensPorGrupoQuery { GrupoId = 1, Pagina = 1, QuantidadeRegistros = 10 };
        var usuarioId = 123;

        _currentUser.Setup(u => u.GetUserId()).Returns(usuarioId);
        _grupoService.Setup(g => g.VerificarUsuarioPertenceAoGrupo(usuarioId, request.GrupoId)).ReturnsAsync(true);
        _readRepository.Setup(r => r.CountAsync(It.IsAny<ListarMensagemBaseSpec>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
        _readRepository.Setup(r => r.ListAsync(It.IsAny<ListarMensagemBaseSpec>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Mensagem>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ResultadoPaginadoViewModel<ListarMensagensViewModel>>();
        result.QuantidadeRegistros.Should().Be(0);
    }
}
