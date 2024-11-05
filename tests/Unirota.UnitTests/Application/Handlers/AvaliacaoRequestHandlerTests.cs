using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Unirota.Application.Commands.Avaliacoes;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Handlers;
using Unirota.Application.Services;
using Unirota.Application.Services.Avaliacoes;
using Xunit;

namespace Unirota.UnitTests.Application.Handlers;

public class AvaliacaoRequestHandlerTests
{
    private readonly Mock<IAvaliacaoService> _avaliacaoService = new();
    private readonly Mock<ICurrentUser> _currentUser = new();
    private readonly Mock<IServiceContext> _serviceContext = new();
    private readonly Mock<IValidator<CriarAvaliacaoCommand>> _validator = new();
    private readonly AvaliacaoRequestHandler _handler;

    public AvaliacaoRequestHandlerTests()
    {
        _handler = new(_avaliacaoService.Object,
                       _currentUser.Object,
                       _serviceContext.Object,
                       _validator.Object);
    }

    [Fact(DisplayName = "Deve criar avaliação e retornar o ID da avaliação quando os dados forem válidos")]
    public async Task DeveCriarAvaliacaoERetornarId_QuandoDadosForemValidos()
    {
        // Arrange
        var usuarioId = 123;
        var expectedAvaliacaoId = 456;

        _validator
            .Setup(v => v.ValidateAsync(It.IsAny<CriarAvaliacaoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _currentUser
            .Setup(c => c.GetUserId())
            .Returns(usuarioId);

        _avaliacaoService
            .Setup(s => s.Criar(It.IsAny<CriarAvaliacaoCommand>(), usuarioId))
            .ReturnsAsync(expectedAvaliacaoId);

        var request = new CriarAvaliacaoCommand();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(expectedAvaliacaoId);
    }

    [Fact(DisplayName = "Deve lançar ValidationException quando os dados da avaliação forem inválidos")]
    public async Task DeveLancarValidationException_QuandoDadosInvalidos()
    {
        // Arrange
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Property", "Error message")
        };

        _validator
            .Setup(v => v.ValidateAsync(It.IsAny<CriarAvaliacaoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var request = new CriarAvaliacaoCommand();

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Validation failed: *");
    }
}
