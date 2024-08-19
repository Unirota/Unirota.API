﻿using FluentValidation;
using Unirota.Application.Requests.Usuarios;

namespace Unirota.Application.Validations.Usuario;

public class CriarUsuarioValidation : AbstractValidator<CriarUsuarioCommand>
{
    public CriarUsuarioValidation()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email é obrigatório")
            .EmailAddress()
            .WithMessage("Email inválido");

        RuleFor(x => x.Senha)
            .NotEmpty()
            .WithMessage("Senha é obrigatória")
            .MinimumLength(6)
            .WithMessage("Senha deve ter no mínimo 6 caracteres");
    }
}
