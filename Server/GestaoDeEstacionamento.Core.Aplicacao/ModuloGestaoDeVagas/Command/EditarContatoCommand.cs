using FluentResults;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Command;
public record EditarVagaCommand(
    Guid id,
    string NomeDaVaga,
    string Zona,
    bool Ocupada,
    Veiculo? Veiculo
    ) : IRequest<IResult<EditarVagaResult>>;

public record EditarVagaResult(
    string NomeDaVaga,
    string Zona,
    bool Ocupada,
    Veiculo? Veiculo
    );