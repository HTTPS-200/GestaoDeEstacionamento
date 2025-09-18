using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloGestaoDeVagas;

public record EditarVagaRequest(
    string NomeDaVaga,
    string Zona,
    bool Ocupada,
    Veiculo? Veiculo
    );

public record EditarVagaResponse(
    string NomeDaVaga,
    string Zona,
    bool Ocupada,
    Veiculo? Veiculo
    );
