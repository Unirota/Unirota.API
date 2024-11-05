using FluentAssertions;
using FluentValidation.TestHelper;
using Unirota.Application.Commands.Grupos;
using Unirota.Application.Validations.Grupos;
using Xunit;

namespace Unirota.UnitTests.Application.Validations;

public class CriarGrupoValidationTests
{
    private readonly CriarGrupoValidation _validator;

    public CriarGrupoValidationTests()
    {
        _validator = new CriarGrupoValidation();
    }

    [Fact(DisplayName = "Deve ser válido quando Nome não é vazio, PassageiroLimite é menor ou igual a 4, HoraInicio é válida e Destino não é vazio")]
    public void DeveSerValido_QuandoNomePassageiroLimiteHoraInicioEDestinoSaoValidos()
    {
        // Arrange
        var command = new CriarGrupoCommand
        {
            Nome = "Grupo de Teste",
            PassageiroLimite = 4,
            HoraInicio = DateTime.Now.AddHours(1),
            Destino = "Destino Válido"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Deve ser inválido quando Nome está vazio")]
    public void DeveSerInvalido_QuandoNomeEstaVazio()
    {
        // Arrange
        var command = new CriarGrupoCommand
        {
            Nome = string.Empty,
            PassageiroLimite = 4,
            HoraInicio = DateTime.Now.AddHours(1),
            Destino = "Destino Válido"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.Nome) &&
                                                       error.ErrorMessage == "Nome é obrigatório");
    }

    [Fact(DisplayName = "Deve ser inválido quando PassageiroLimite é maior que 4")]
    public void DeveSerInvalido_QuandoPassageiroLimiteMaiorQueQuatro()
    {
        // Arrange
        var command = new CriarGrupoCommand
        {
            Nome = "Grupo de Teste",
            PassageiroLimite = 5,
            HoraInicio = DateTime.Now.AddHours(1),
            Destino = "Destino Válido"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.PassageiroLimite) &&
                                                       error.ErrorMessage == "Não é possível criar um grupo com mais de 4 passageiros");
    }

    [Fact(DisplayName = "Deve ser inválido quando HoraInicio é uma data inválida")]
    public void DeveSerInvalido_QuandoHoraInicioInvalida()
    {
        // Arrange
        var command = new CriarGrupoCommand
        {
            Nome = "Grupo de Teste",
            PassageiroLimite = 4,
            HoraInicio = default,
            Destino = "Destino Válido"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.HoraInicio) &&
                                                       error.ErrorMessage == "Hora de início inválida");
    }

    [Fact(DisplayName = "Deve ser inválido quando Destino está vazio")]
    public void DeveSerInvalido_QuandoDestinoEstaVazio()
    {
        // Arrange
        var command = new CriarGrupoCommand
        {
            Nome = "Grupo de Teste",
            PassageiroLimite = 4,
            HoraInicio = DateTime.Now.AddHours(1),
            Destino = string.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.Destino) &&
                                                       error.ErrorMessage == "Insira um destino válido");
    }
}
