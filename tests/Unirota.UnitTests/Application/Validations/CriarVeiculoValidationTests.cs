using FluentAssertions;
using FluentValidation.TestHelper;
using Unirota.Application.Commands.Veiculos;
using Unirota.Application.Validations.Veiculos;
using Xunit;

namespace Unirota.UnitTests.Application.Validations;

public class CriarVeiculoValidationTests
{
    private readonly CriarVeiculoValidation _validator;

    public CriarVeiculoValidationTests()
    {
        _validator = new CriarVeiculoValidation();
    }

    [Fact(DisplayName = "Deve ser válido quando todas as propriedades são válidas")]
    public void DeveSerValido_QuandoTodasAsPropriedadesSaoValidas()
    {
        // Arrange
        var command = new CriarVeiculosCommand
        {
            Placa = "ABC-1234",
            Cor = "Vermelho",
            Carroceria = "Sedan"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "Deve ser inválido quando Placa está vazia")]
    [InlineData("")]
    [InlineData(null)]
    public void DeveSerInvalido_QuandoPlacaEstaVazia(string placa)
    {
        // Arrange
        var command = new CriarVeiculosCommand
        {
            Placa = placa,
            Cor = "Vermelho",
            Carroceria = "Sedan"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.Placa) &&
                                                       error.ErrorMessage == "Placa é obrigatório");
    }

    [Theory(DisplayName = "Deve ser inválido quando Cor está vazia")]
    [InlineData("")]
    [InlineData(null)]
    public void DeveSerInvalido_QuandoCorEstaVazia(string cor)
    {
        // Arrange
        var command = new CriarVeiculosCommand
        {
            Placa = "ABC-1234",
            Cor = cor,
            Carroceria = "Sedan"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.Cor) &&
                                                       error.ErrorMessage == "A cor é obrigatória");
    }

    [Theory(DisplayName = "Deve ser inválido quando Carroceria está vazia")]
    [InlineData("")]
    [InlineData(null)]
    public void DeveSerInvalido_QuandoCarroceriaEstaVazia(string carroceria)
    {
        // Arrange
        var command = new CriarVeiculosCommand
        {
            Placa = "ABC-1234",
            Cor = "Vermelho",
            Carroceria = carroceria
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.Carroceria) &&
                                                       error.ErrorMessage == "A carroceria é obrigatória");
    }
}
