using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Command;

namespace GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloGestaoDeVagas;
public class CadastrarVagaCommandValidator : AbstractValidator<CadastrarVagaCommand>
{
    public CadastrarVagaCommandValidator()
    {
        RuleFor(x => x.NumeroDaVaga)
            .NotEmpty().WithMessage("O Numero da Vaga é Obrigatorio.")
            .Matches("^[A-Za-z]{1}[0-9]{2}$").WithMessage("Formato aceito A12.")
            .MinimumLength(2).WithMessage("A Vaga deve conter no minimo {MinLength} caracteres")
            .MaximumLength(4).WithMessage("A vaga não pode conter mais de {MaxLength} caracteres");

        RuleFor(x => x.Zona)
            .NotEmpty().WithMessage("A Zona onde a vaga está localizada é Obrigatoria.");

        RuleFor(x => x.Ocupada).Null();
    }
}
