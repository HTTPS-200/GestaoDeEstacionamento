using FluentResults;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using MediatR;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Command;
public record SelecionarVagasQuery(int? Quantidade) : IRequest<Result<SelecionarVagasResult>>;

public record SelecionarVagasResult(ImmutableList<SelecionarVagasDto> Vagas);

public record SelecionarVagasDto(
    Guid Id,
    string NumeroDaVaga,
    string Zona,
    bool Ocupada,
    string? VeiculoId
    );