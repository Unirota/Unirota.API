using FluentAssertions;
using Mapster;
using Moq;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Usuario;
using Unirota.Application.Requests.Enderecos;
using Unirota.Application.Services;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Application.ViewModels.Auth;
using Unirota.Application.ViewModels.Usuarios;
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

    [Fact(DisplayName = "Handle login deve token quando usuario for valido")]
    public async Task HandleLogin_ShouldReturnToken()
    {
        // Arrange
        var loginCommand = new LoginCommand
        {
            Email = "",
            Senha = ""
        };

        var usuario = new UsuarioBuilder().Build();
        var token = new TokenViewModel
        {
            AccessToken = "",
            Usuario = usuario.Adapt<UsuarioViewModel>(),
        };
        _readRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorEmailSpec>(), CancellationToken.None))
            .ReturnsAsync(usuario);
        _jwtProvider.Setup(x => x.GerarToken(It.IsAny<UsuarioViewModel>()))
            .Returns(token);
        _service.Setup(x => x.ValidarSenha(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);


        // Act
        var result = await _handler.Handle(loginCommand, CancellationToken.None);

        // Assert
        result.Should().BeOfType<TokenViewModel>();
        _jwtProvider.Invocations.Should().ContainSingle(invocation => invocation.Method.Name == nameof(_jwtProvider.Object.GerarToken));
    }

    [Fact(DisplayName = "Handle login deve valor default quando usuario não existir")]
    public async Task HandleLogin_ShouldReturnDefaultValue_WhenUsuarioIsNull()
    {
        // Arrange
        var loginCommand = new LoginCommand
        {
            Email = "",
            Senha = ""
        };

        // Act
        var result = await _handler.Handle(loginCommand, CancellationToken.None);

        // Assert
        result.Should().Be(default);
        _serviceContext.Verify(x => x.AddError("Usuário não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Handle login deve erro quando senha for invalido")]
    public async Task HandleLogin_ShouldReturnDefaultValue_WhenPasswordIsNull()
    {
        // Arrange
        var loginCommand = new LoginCommand
        {
            Email = "",
            Senha = ""
        };

        var usuario = new UsuarioBuilder().Build();

        _readRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorEmailSpec>(), CancellationToken.None))
            .ReturnsAsync(usuario);
        _service.Setup(x => x.ValidarSenha(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        // Act
        var result = await _handler.Handle(loginCommand, CancellationToken.None);

        // Assert
        result.Should().Be(default);
        _serviceContext.Verify(x => x.AddError("Senha inválida"), Times.Once);
    }

    [Fact(DisplayName = "Handle editar deve chamar service e retornar objeto")]
    public async Task HandleEditar_ShouldCallEditarAndReturnObject()
    {
        //Arrange
        var request = new EditarUsuarioCommand
        {
            Id = 1,
            Nome = "Natan",
            DataNascimento = DateTime.Now,
            Senha = "coxinha123",
            ImagemUrl = "",
            Endereco = new EditarEnderecoRequest
            {
                CEP = "12345678",
                Bairro = "Bairro",
                Cidade = "Cidade",
                Complemento = "Complemento",
                Logradouro = "Logradouro",
                Numero = 123,
                UF = "SP"
            }
        };

        var usuario = new UsuarioBuilder().Build();
        _service.Setup(x => x.Editar(It.IsAny<EditarUsuarioCommand>(), CancellationToken.None))
            .ReturnsAsync(usuario.Adapt<UsuarioViewModel>());

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        result.Should().BeOfType<UsuarioViewModel>();
    }

    [Fact(DisplayName = "Handle consultar por Id deve chamar service e retornar objeto")]
    public async Task HandleConsultarPorId_ShouldCallEditarAndReturnObject()
    {
        //Arrange
        var request = new ConsultarUsuarioPorIdQuery
        {
            Id = 1,
        };

        var usuario = new UsuarioBuilder().Build();
        _service.Setup(x => x.ConsultarPorId(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(usuario.Adapt<UsuarioViewModel>());

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        result.Should().BeOfType<UsuarioViewModel>();
    }
}
