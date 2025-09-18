using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloGestaoDeVagas;

public record CadastrarVagaRequest(
        string NumeroDaVaga,
        string Zona,
        bool Ocupada,
        string VeiculoId
    );

public record CadastrarVagaResponse(Guid id);

