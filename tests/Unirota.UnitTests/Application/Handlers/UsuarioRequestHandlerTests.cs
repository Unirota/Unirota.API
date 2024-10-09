using FluentAssertions;
using Moq;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers;
using Unirota.Application.Persistence;
using Unirota.Application.Services;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Domain.Entities.Usuarios;
using Unirota.UnitTests.Builder;
using Xunit;

namespace Unirota.UnitTests.Application.Handlers;

public class UsuarioRequestHandlerTests
{
    private readonly Mock<IServiceContext> _serviceContext = new();
    private readonly Mock<IRepository<Usuario>> _repository = new();
    private readonly Mock<IReadRepository<Usuario>> _readRepository = new();
    private readonly Mock<IUsuarioService> _service = new();
    private readonly Mock<IJwtProvider> _jwtProvider = new();

    private readonly UsuarioRequestHandler _handler;

    public UsuarioRequestHandlerTests()
    {
        _handler = new UsuarioRequestHandler(_serviceContext.Object, 
                                             _repository.Object,
                                             _readRepository.Object,
                                             _service.Object,
                                             _jwtProvider.Object);
    }

    [Fact(DisplayName = "Criar usuário deve criar usuário e retornar um Id")]
    public async Task Handle_ShouldCreateUsuarioAndReturnId()
    {
        //Arrange
        var request = new CriarUsuarioCommand
        {
            Nome = "Natan",
            Email = "natan@gmail.com",
            Senha = "coxinha123",
            CPF = "12053580989",
            DataNascimento = DateTime.Now
        };

        _service.Setup(x => x.ValidarCpf(It.IsAny<string>()))
            .Returns("Ok");

        var usuario = new UsuarioBuilder().Build();
        _readRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorCPFSpec>(), It.IsAny<CancellationToken>()));
        _readRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);
        _repository.Setup(x => x.AddAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);
        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        result.Should().BeGreaterThanOrEqualTo(0);
    }
}
