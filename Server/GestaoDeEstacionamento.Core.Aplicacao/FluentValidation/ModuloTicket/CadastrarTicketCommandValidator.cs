using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Validators;

public class CadastrarTicketCommandValidator : AbstractValidator<CadastrarTicketCommand>
{
    public CadastrarTicketCommandValidator()
    {
        RuleFor(x => x.PlacaVeiculo)
            .NotEmpty().WithMessage("Placa do veículo é obrigatória")
            .MaximumLength(10).WithMessage("Placa deve ter no máximo 10 caracteres")
            .Matches(@"^[A-Za-z0-9]{3,10}$").WithMessage("Placa deve conter apenas letras e números");
    }
}

public class EditarTicketCommandValidator : AbstractValidator<EditarTicketCommand>
{
    public EditarTicketCommandValidator()
    {
        RuleFor(x => x.PlacaVeiculo)
           .NotEmpty().WithMessage("Placa do veículo é obrigatória")
           .MaximumLength(10).WithMessage("Placa deve ter no máximo 10 caracteres")
           .Matches(@"^[A-Za-z0-9]{3,10}$").WithMessage("Placa deve conter apenas letras e números");
    }
}

public class ObterTicketPorNumeroQueryValidator : AbstractValidator<ObterTicketPorNumeroQuery>
{
    public ObterTicketPorNumeroQueryValidator()
    {
        RuleFor(x => x.NumeroTicket)
            .NotEmpty().WithMessage("Placa do veículo é obrigatória")
            .MaximumLength(10).WithMessage("Placa deve ter no máximo 10 caracteres")
            .Matches(@"^[A-Za-z0-9]{3,10}$").WithMessage("Placa deve conter apenas letras e números");
    }
}