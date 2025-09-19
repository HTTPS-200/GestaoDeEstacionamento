using FluentValidation;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Validators;

public class VagaValidator : AbstractValidator<Vaga>
{
    public VagaValidator()
    {
        RuleFor(v => v.Identificador)
            .NotEmpty()
            .WithMessage("Identificador da vaga é obrigatório")
            .MaximumLength(20)
            .WithMessage("Identificador deve ter no máximo 20 caracteres");

        RuleFor(v => v.Zona)
            .NotEmpty()
            .WithMessage("Zona da vaga é obrigatória")
            .MaximumLength(50)
            .WithMessage("Zona deve ter no máximo 50 caracteres");
    }
}