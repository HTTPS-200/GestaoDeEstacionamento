using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloCheckIn;

public class CadastrarVeiculoCommandValidator : AbstractValidator<CadastrarVeiculoCommand>
{
    public CadastrarVeiculoCommandValidator()
    {
        RuleFor(x => x.Placa)
            .NotEmpty().WithMessage("Placa é obrigatória")
            .MaximumLength(10).WithMessage("Placa deve ter no máximo 10 caracteres");

        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("Modelo é obrigatório")
            .MaximumLength(50).WithMessage("Modelo deve ter no máximo 50 caracteres");

        RuleFor(x => x.Cor)
            .NotEmpty().WithMessage("Cor é obrigatória")
            .MaximumLength(20).WithMessage("Cor deve ter no máximo 20 caracteres");

        RuleFor(x => x.CPFHospede)
            .NotEmpty().WithMessage("CPF do hóspede é obrigatório")
            .MaximumLength(14).WithMessage("CPF deve ter no máximo 14 caracteres");

        RuleFor(x => x.Observacoes)
            .MaximumLength(500).WithMessage("Observações deve ter no máximo 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Observacoes));
    }
}