using FluentResults;
using MediatR;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;

public record CriarVagaCommand(
    string Identificador,
    string Zona,
    Guid UsuarioId 
) : IRequest<Result<CriarVagaResult>>;

public record CriarVagaResult(
    Guid Id,
    string Identificador,
    string Zona,
    bool Ocupada
);

public record EditarVagaCommand(
    Guid Id,
    string Identificador,
    string Zona,
    bool Ocupada,
    Guid? VeiculoId
) : IRequest<Result<EditarVagaResult>>;

public record EditarVagaResult(
    Guid Id,
    string Identificador,
    string Zona,
    bool Ocupada,
    Guid? VeiculoId
);

public record ExcluirVagaCommand(Guid Id) : IRequest<Result<ExcluirVagaResult>>;
public record ExcluirVagaResult();

public record ObterVagaPorIdQuery(Guid Id) : IRequest<Result<ObterVagaPorIdResult>>;

public record ObterVagaPorIdResult(
    Guid Id,
    string Identificador,
    string Zona,
    bool Ocupada,
    Guid? VeiculoId
);

public record ObterTodasVagasQuery() : IRequest<Result<ObterTodasVagasResult>>;

public record ObterTodasVagasResult(ImmutableList<ObterVagaItemResult> Vagas);

public record ObterVagaItemResult(
    Guid Id,
    string Identificador,
    string Zona,
    bool Ocupada,
    Guid? VeiculoId
);

public record ObterVagasLivresQuery() : IRequest<Result<ObterTodasVagasResult>>;
public record ObterVagasOcupadasQuery() : IRequest<Result<ObterTodasVagasResult>>;

public record OcuparVagaCommand(
    Guid VagaId,
    string PlacaVeiculo
) : IRequest<Result<OcuparVagaResult>>;

public record OcuparVagaResult(
    Guid VagaId,
    string Identificador,
    bool Ocupada,
    Guid VeiculoId,
    string PlacaVeiculo
);

public record LiberarVagaCommand(Guid VagaId) : IRequest<Result<LiberarVagaResult>>;

public record LiberarVagaResult(
    Guid VagaId,
    string Identificador,
    bool Ocupada
);