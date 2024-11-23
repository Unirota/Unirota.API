using FluentAssertions;
using FluentValidation.TestHelper;
using Unirota.Application.Commands.Avaliacoes;
using Unirota.Application.Validations.Avaliacoes;
using Xunit;

namespace Unirota.UnitTests.Application.Validations;

public class CriarAvaliacaoValidationTests
{
    private readonly CriarAvaliacaoValidation _validator;

    public CriarAvaliacaoValidationTests()
    {
        _validator = new CriarAvaliacaoValidation();
    }

    [Fact(DisplayName = "Deve ser válido quando Nota está entre 1 e 5 e CorridaId é maior que 0")]
    public void DeveSerValido_QuandoNotaEIdCorridaSaoValidos()
    {
        // Arrange
        var command = new CriarAvaliacaoCommand { Nota = 3, CorridaId = 1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Deve ser inválido quando Nota é menor que 1")]
    public void DeveSerInvalido_QuandoNotaMenorQueUm()
    {
        // Arrange
        var command = new CriarAvaliacaoCommand { Nota = 0, CorridaId = 1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.Nota) &&
                                                       error.ErrorMessage == "A nota deve ser entre 1 e 5");
    }

    [Fact(DisplayName = "Deve ser inválido quando Nota é maior que 5")]
    public void DeveSerInvalido_QuandoNotaMaiorQueCinco()
    {
        // Arrange
        var command = new CriarAvaliacaoCommand { Nota = 6, CorridaId = 1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.Nota) &&
                                                       error.ErrorMessage == "A nota deve ser entre 1 e 5");
    }

    [Fact(DisplayName = "Deve ser inválido quando CorridaId é menor ou igual a 0")]
    public void DeveSerInvalido_QuandoCorridaIdMenorOuIgualAZero()
    {
        // Arrange
        var command = new CriarAvaliacaoCommand { Nota = 3, CorridaId = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.CorridaId) &&
                                                       error.ErrorMessage == "CorridaId inválido");
    }
}
