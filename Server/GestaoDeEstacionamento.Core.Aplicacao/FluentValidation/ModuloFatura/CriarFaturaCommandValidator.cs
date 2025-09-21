using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Validators;

public class CriarFaturaCommandValidator : AbstractValidator<CriarFaturaCommand>
{
    public CriarFaturaCommandValidator()
    {
        RuleFor(x => x.CheckInId)
            .NotEmpty().WithMessage("CheckInId é obrigatório");

        RuleFor(x => x.VeiculoId)
            .NotEmpty().WithMessage("VeiculoId é obrigatório");

        RuleFor(x => x.TicketId)
            .NotEmpty().WithMessage("TicketId é obrigatório");

        RuleFor(x => x.NumeroTicket)
            .NotEmpty().WithMessage("Número do ticket é obrigatório")
            .MaximumLength(20).WithMessage("Número do ticket deve ter no máximo 20 caracteres");

        RuleFor(x => x.PlacaVeiculo)
            .NotEmpty().WithMessage("Placa do veículo é obrigatória")
            .MaximumLength(10).WithMessage("Placa deve ter no máximo 10 caracteres");

        RuleFor(x => x.ModeloVeiculo)
            .NotEmpty().WithMessage("Modelo do veículo é obrigatório")
            .MaximumLength(50).WithMessage("Modelo deve ter no máximo 50 caracteres");

        RuleFor(x => x.CorVeiculo)
            .NotEmpty().WithMessage("Cor do veículo é obrigatória")
            .MaximumLength(20).WithMessage("Cor deve ter no máximo 20 caracteres");

        RuleFor(x => x.CPFHospede)
            .NotEmpty().WithMessage("CPF do hóspede é obrigatório")
            .MaximumLength(14).WithMessage("CPF deve ter no máximo 14 caracteres");

        RuleFor(x => x.DataHoraEntrada)
            .NotEmpty().WithMessage("Data/hora de entrada é obrigatória")
            .LessThan(x => x.DataHoraSaida).WithMessage("Data de entrada deve ser anterior à data de saída");

        RuleFor(x => x.DataHoraSaida)
            .NotEmpty().WithMessage("Data/hora de saída é obrigatória")
            .GreaterThan(x => x.DataHoraEntrada).WithMessage("Data de saída deve ser posterior à data de entrada");

        RuleFor(x => x.Diarias)
            .GreaterThan(0).WithMessage("Número de diárias deve ser maior que zero");

        RuleFor(x => x.ValorDiaria)
            .GreaterThan(0).WithMessage("Valor da diária deve ser maior que zero");

        RuleFor(x => x.ValorTotal)
            .GreaterThan(0).WithMessage("Valor total deve ser maior que zero")
            .Equal(x => x.Diarias * x.ValorDiaria).WithMessage("Valor total deve ser igual a diárias × valor diária");
    }
}