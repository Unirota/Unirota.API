using FluentAssertions;
using FluentValidation.TestHelper;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Validations.Usuarios;
using Xunit;

namespace Unirota.UnitTests.Application.Validations;

public class CriarUsuarioValidationTests
{
    private readonly CriarUsuarioValidation _validator;

    public CriarUsuarioValidationTests()
    {
        _validator = new CriarUsuarioValidation();
    }

    [Fact(DisplayName = "Deve ser válido quando todas as propriedades são válidas")]
    public void DeveSerValido_QuandoTodasAsPropriedadesSaoValidas()
    {
        // Arrange
        var command = new CriarUsuarioCommand
        {
            Nome = "Nome Válido",
            Email = "usuario@exemplo.com",
            Senha = "senha123",
            CPF = "12345678901",
            DataNascimento = new DateTime(2000, 1, 1)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "Deve ser inválido quando Nome está vazio")]
    [InlineData("")]
    [InlineData(null)]
    public void DeveSerInvalido_QuandoNomeEstaVazio(string nome)
    {
        // Arrange
        var command = new CriarUsuarioCommand
        {
            Nome = nome,
            Email = "usuario@exemplo.com",
            Senha = "senha123",
            CPF = "12345678901",
            DataNascimento = new DateTime(2000, 1, 1)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.Nome) &&
                                                       error.ErrorMessage == "Nome é obrigatório");
    }

    [Theory(DisplayName = "Deve ser inválido quando Email está vazio ou inválido")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("emailinvalido")]
    public void DeveSerInvalido_QuandoEmailEstaVazioOuInvalido(string email)
    {
        // Arrange
        var command = new CriarUsuarioCommand
        {
            Nome = "Nome Válido",
            Email = email,
            Senha = "senha123",
            CPF = "12345678901",
            DataNascimento = new DateTime(2000, 1, 1)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        if (string.IsNullOrEmpty(email))
        {
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.Email) &&
                                                           error.ErrorMessage == "Email é obrigatório");
        }
        else
        {
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.Email) &&
                                                           error.ErrorMessage == "Email inválido");
        }
    }

    [Theory(DisplayName = "Deve ser inválido quando Senha está vazia ou tem menos de 6 caracteres")]
    [InlineData("")]
    [InlineData("12345")]
    public void DeveSerInvalido_QuandoSenhaEstaVaziaOuInvalida(string senha)
    {
        // Arrange
        var command = new CriarUsuarioCommand
        {
            Nome = "Nome Válido",
            Email = "usuario@exemplo.com",
            Senha = senha,
            CPF = "12345678901",
            DataNascimento = new DateTime(2000, 1, 1)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        if (string.IsNullOrEmpty(senha))
        {
            result.Errors.Should().ContainSingle(error => error.ErrorMessage == "Senha é obrigatória");
        }
        else
        {
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.Senha) &&
                                                           error.ErrorMessage == "Senha deve ter no mínimo 6 caracteres");
        }
    }
}
