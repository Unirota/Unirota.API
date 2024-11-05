using FluentAssertions;
using Moq;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Persistence;
using Unirota.Application.Requests.Enderecos;
using Unirota.Application.Services;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Application.ViewModels.Usuarios;
using Unirota.Domain.Entities.Usuarios;
using Unirota.UnitTests.Builder;
using Xunit;

namespace Unirota.UnitTests.Application.Services;

public class UsuarioServiceTests
{
    private Mock<IRepository<Usuario>> _repository = new();
    private Mock<ICurrentUser> _currentUser = new();
    private Mock<IServiceContext> _serviceContext = new();

    public UsuarioService _service;

    public UsuarioServiceTests()
    {
        _service = new UsuarioService(_repository.Object, _currentUser.Object, _serviceContext.Object);
    }

    [Fact(DisplayName = "Validar CPF deve retornar Ok quando CPF for válido.")]
    public void ValidarCpf_ShouldReturnOk_WhenCpfIsValid()
    {
        // Arrange
        string validCpf = "12345678909";

        // Act
        var result = _service.ValidarCpf(validCpf);

        // Assert
        result.Should().Be("Ok");
    }

    [Fact(DisplayName = "Validar CPF deve retornar erro quando CPF for invalido com mesmos digitos.")]
    public void ValidarCpf_ShouldReturnError_WhenCpfIsInvalidWithSameDigits()
    {
        // Arrange
        string validCpf = "11111111111";

        // Act
        var result = _service.ValidarCpf(validCpf);

        // Assert
        result.Should().Be("CPF inválido: todos os dígitos são iguais");
    }

    [Fact(DisplayName = "Validar CPF deve retornar erro quando CPF for invalido.")]
    public void ValidarCpf_ShouldReturnOkError_WhenCpfIsInvalid()
    {
        // Arrange
        string validCpf = "12345678900";

        // Act
        var result = _service.ValidarCpf(validCpf);

        // Assert
        result.Should().Be("CPF inválido");
    }

    [Fact(DisplayName = "Editar deve retornar null quando usuário não for o mesmo que o da requisição.")]
    public async Task Editar_ShouldReturnNull_WhenUserIsInvalid()
    {
        // Arrange
        var request = new EditarUsuarioCommand
        {
            Id = 1,
        };
        _currentUser.Setup(x => x.GetUserId())
            .Returns(0);

        // Act
        var result = await _service.Editar(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _serviceContext.Invocations.Should().ContainSingle(x => x.Method.Name == nameof(_serviceContext.Object.AddError));
    }

    [Fact(DisplayName = "Editar deve retornar null quando usuário não estiver cadastrado.")]
    public async Task Editar_ShouldReturnNull_WhenUserIsNotRegistered()
    {
        // Arrange
        var request = new EditarUsuarioCommand
        {
            Id = 1
        };
        
        _currentUser.Setup(x => x.GetUserId())
            .Returns(1);

        // Act
        var result = await _service.Editar(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _serviceContext.Verify(x => x.AddError("Usuário não cadastrado."), Times.Once);
        _serviceContext.Invocations.Should().Contain(x => x.Method.Name == nameof(_serviceContext.Object.AddError));
    }

    [Fact(DisplayName = "Editar deve retornar view model quando requisição for válida.")]
    public async Task Editar_ShouldReturnViewModel_WhenRequestIsValid()
    {
        // Arrange
        var request = new EditarUsuarioCommand
        {
            Id = 1,
            Nome = "Natan",
            DataNascimento = new DateTime(2022, 3, 17),
            Senha = "coxinha123",
            ImagemUrl = "https://static.wikia.nocookie.net/swordartonline/images/8/87/Kirito_Dual_Blades.png",
            Endereco = new EditarEnderecoRequest
            {
                Logradouro = "Rua Unicesumar",
                Bairro = "Bloco 10",
                Numero = 123,
                Complemento = "complemento",
                CEP = "87100000",
                Cidade = "Maringá"
            }
        };

        var usuario = new UsuarioBuilder().Build();

        _currentUser.Setup(x => x.GetUserId())
            .Returns(1);
        _repository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), CancellationToken.None))
            .ReturnsAsync(usuario);


        // Act
        var result = await _service.Editar(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<UsuarioViewModel>();
        _repository.Invocations.Should().Contain(x => x.Method.Name == nameof(_repository.Object.SaveChangesAsync));
    }

    [Fact(DisplayName = "Consultar usuario por id deve retornar erro quando usuário não existir")]
    public async Task ConsultarUsuarioPorId_ShouldReturnError_WhenUserIsInvalid()
    {
        //Arrange
        int usuarioId = 0;

        // Act
        var result = await _service.ConsultarPorId(usuarioId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _serviceContext.Invocations.Should().ContainSingle(x => x.Method.Name == nameof(_serviceContext.Object.AddError));
        _serviceContext.Verify(x => x.AddError("Usuário não existente"), Times.Once);
    }

    [Fact(DisplayName = "Consultar usuario por id deve retornar view model quando usuário for válido")]
    public async Task ConsultarUsuarioPorId_ShouldReturnUser_WhenUserIsValid()
    {
        //Arrange
        int usuarioId = 0;
        var usuario = new UsuarioBuilder().Build();

        _repository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), CancellationToken.None))
            .ReturnsAsync(usuario);

        // Act
        var result = await _service.ConsultarPorId(usuarioId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<UsuarioViewModel>();
    }

    [Fact(DisplayName = "Verificar usuario existe deve retornar true quando usuário existir")]
    public async Task VerificarUsuarioId_ShouldReturnTrue_WhenUserIsValid()
    {
        //Arrange
        int usuarioId = 0;
        var usuario = new UsuarioBuilder().Build();

        _repository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ConsultarUsuarioPorIdSpec>(), CancellationToken.None))
            .ReturnsAsync(usuario);

        // Act
        var result = await _service.VerificarUsuarioExiste(usuarioId);

        // Assert
        Assert.True(result);
    }
}
