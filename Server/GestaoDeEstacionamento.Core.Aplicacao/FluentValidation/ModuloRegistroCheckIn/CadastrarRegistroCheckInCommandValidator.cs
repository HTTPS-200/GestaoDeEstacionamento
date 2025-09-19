using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloCheckIn
{
    public class RealizarCheckInCommandValidator : AbstractValidator<RealizarCheckInCommand>
    {
        public RealizarCheckInCommandValidator()
        {
            RuleFor(x => x.PlacaVeiculo)
                .NotEmpty().WithMessage("A placa do veículo é obrigatória.");

            RuleFor(x => x.CPFHospede)
                .NotEmpty().WithMessage("O CPF do hóspede é obrigatório.");

        }
    }
}
