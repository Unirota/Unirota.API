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
using Unirota.Application.Requests.Enderecos;
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
        var endereco = new EnderecoBuilder().Build();
        var usuario = new UsuarioBuilder().WithEndereco(endereco).Build();
        var request = new CriarUsuarioCommand
        {
            Nome = "Natan",
            Email = "natan@gmail.com",
            Senha = "coxinha123",
            CPF = "12053580989",
            DataNascimento = DateTime.Now,
            Endereco = new CriarEnderecoRequest
            {
                CEP = endereco.CEP,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Complemento = endereco.Complemento,
                UF = endereco.UF,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero
            }
        };

        _service.Setup(x => x.ValidarCpf(It.IsAny<string>()))
            .Returns("Ok");

        _readRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorCPFSpec>(), It.IsAny<CancellationToken>()));
        _readRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);
        _repository.Setup(r => r.AddAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);
        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        result.Should().Be(usuario.Id);
        _repository.Invocations.Should().ContainSingle(invocation => invocation.Method.Name == nameof(_repository.Object.AddAsync));
    }

    [Fact(DisplayName = "Criar usuário com CPF invalido deve retornar erro")]
    public async Task Handle_ShouldNotCreateUsuarioAndReturnError()
    {
        //Arrange
        var endereco = new EnderecoBuilder().Build();
        var usuario = new UsuarioBuilder().WithEndereco(endereco).Build();
        var request = new CriarUsuarioCommand
        {
            Nome = "Natan",
            Email = "natan@gmail.com",
            Senha = "coxinha123",
            CPF = "12053580989",
            DataNascimento = DateTime.Now,
            Endereco = new CriarEnderecoRequest
            {
                CEP = endereco.CEP,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Complemento = endereco.Complemento,
                UF = endereco.UF,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero
            }
        };

        _service.Setup(x => x.ValidarCpf(It.IsAny<string>()))
            .Returns("CPF inválido");
        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        result.Should().Be(default); 
        _serviceContext.Verify(sc => sc.AddError("CPF inválido"), Times.Once); 
        _repository.Invocations.Should().BeEmpty();
    }

    [Fact(DisplayName = "Handle deve retornar erro quando CPF for duplicado")]
    public async Task Handle_ShouldReturnErrorWhenCpfIsDuplicated()
    {
        // Arrange
        var endereco = new EnderecoBuilder().Build();
        var usuarioDuplicado = new UsuarioBuilder().Build();
        var request = new CriarUsuarioCommand
        {
            Nome = "Natan",
            Email = "natan@gmail.com",
            Senha = "coxinha123",
            CPF = "12053580989",
            DataNascimento = DateTime.Now,
            Endereco = new CriarEnderecoRequest
            {
                CEP = endereco.CEP,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Complemento = endereco.Complemento,
                UF = endereco.UF,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero
            }
        };

        _service.Setup(x => x.ValidarCpf(It.IsAny<string>()))
                .Returns("Ok");

        _readRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorCPFSpec>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(usuarioDuplicado);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(default); 
        _serviceContext.Verify(sc => sc.AddError("CPF duplicado!"), Times.Once); 
        _repository.Invocations.Should().BeEmpty();
    }
}
