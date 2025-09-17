using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloVeiculo
{
    public class EditarVeiculoCommandValidator : AbstractValidator<EditarVeiculoCommand>
    {
        public EditarVeiculoCommandValidator()
        {
            RuleFor(x => x.Ticket)
               .NotEmpty().WithMessage("Ticket inválido.");

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
}