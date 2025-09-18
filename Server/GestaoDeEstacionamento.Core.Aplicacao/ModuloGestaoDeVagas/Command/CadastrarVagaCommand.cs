using FluentResults;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Command;
public record CadastrarVagaCommand(
        string NumeroDaVaga,
        string Zona,
        bool Ocupada,
        string? VeiculoId
    ) : IRequest<Result<CadastrarVagaResult>>;

public class CadastrarVagaResult(Guid Id);