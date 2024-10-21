using FluentValidation;
using Unirota.Application.Commands.Avaliacoes;

namespace Unirota.Application.Validations.Avaliacoes;

public class CriarAvaliacaoValidation : AbstractValidator<CriarAvaliacaoCommand>
{
    public CriarAvaliacaoValidation()
    {
        RuleFor(x => x.Nota)
            .InclusiveBetween(1, 5)
            .WithMessage("A nota deve ser entre 1 e 5");

        RuleFor(x => x.CorridaId)
            .GreaterThan(0)
            .WithMessage("CorridaId inválido");
    }
}