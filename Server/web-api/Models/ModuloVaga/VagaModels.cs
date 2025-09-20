using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloVaga;

public record CriarVagaRequest(
    string Identificador,
    string Zona
);

public record CriarVagaResponse(
    Guid Id,
    string Identificador,
    string Zona,
    bool Ocupada
);

public record EditarVagaRequest(
    string Identificador,
    string Zona,
    bool Ocupada
);

public record EditarVagaResponse(
    Guid Id,
    string Identificador,
    string Zona,
    bool Ocupada,
    Guid? VeiculoId
);

public record ObterVagaPorIdResponse(
    Guid Id,
    string Identificador,
    string Zona,
    bool Ocupada,
    Guid? VeiculoId
);

public record ObterTodasVagasResponse(
    ImmutableList<ObterVagaItemResponse> Vagas
);

public record ObterVagaItemResponse(
    Guid Id,
    string Identificador,
    string Zona,
    bool Ocupada,
    Guid? VeiculoId
);

public record OcuparVagaRequest(string PlacaVeiculo);

public record OcuparVagaResponse(
    Guid VagaId,
    string Identificador,
    bool Ocupada,
    Guid VeiculoId,
    string PlacaVeiculo
);

public record LiberarVagaResponse(
    Guid VagaId,
    string Identificador,
    bool Ocupada
);