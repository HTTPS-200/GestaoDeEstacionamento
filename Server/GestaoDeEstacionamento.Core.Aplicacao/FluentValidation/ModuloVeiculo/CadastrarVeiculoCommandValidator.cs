using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloCheckIn;

public class CadastrarVeiculoCommandValidator : AbstractValidator<CadastrarVeiculoCommand>
{
    public CadastrarVeiculoCommandValidator()
    {
        RuleFor(x => x.Placa)
            .NotEmpty().WithMessage("A placa é obrigatória.")
            .MaximumLength(10);

        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("O modelo é obrigatório.")
            .MaximumLength(50);

        RuleFor(x => x.Cor)
            .NotEmpty().WithMessage("A cor é obrigatória.")
            .MaximumLength(30);

        RuleFor(x => x.CpfHospede)
            .NotEmpty().WithMessage("O CPF do hóspede é obrigatório.")
            .MaximumLength(14);
    }
}