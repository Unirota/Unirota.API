using FluentAssertions;
using FluentValidation.TestHelper;
using Unirota.Application.Commands.Mensagens;
using Unirota.Application.Validations.Mensagens;
using Xunit;

namespace Unirota.UnitTests.Application.Validations;

public class CriarMensagemValidationTests
{
    private readonly CriarMensagemValidation _validator;

    public CriarMensagemValidationTests()
    {
        _validator = new CriarMensagemValidation();
    }

    [Fact(DisplayName = "Deve ser válido quando Conteudo não está vazio e tem até 512 caracteres")]
    public void DeveSerValido_QuandoConteudoNaoEstaVazioEComMaximoDe512Caracteres()
    {
        // Arrange
        var command = new CriarMensagemCommand
        {
            Conteudo = "Mensagem válida com conteúdo permitido."
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Deve ser inválido quando Conteudo tem mais de 512 caracteres")]
    public void DeveSerInvalido_QuandoConteudoTemMaisDe512Caracteres()
    {
        // Arrange
        var command = new CriarMensagemCommand
        {
            Conteudo = new string('a', 513)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(command.Conteudo) &&
                                                       error.ErrorMessage == "A mensagem não pode ter mais de 512 caracteres");
    }
}
