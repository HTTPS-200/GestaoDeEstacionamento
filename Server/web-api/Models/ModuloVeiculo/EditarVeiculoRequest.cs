namespace GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo;

public record EditarVeiculoRequest(
    string Placa,
    string Modelo,
    string Cor,
    string CPFHospede,
    string? Observacoes = null
);

public record EditarVeiculoResponse(
    string Placa,
    string Modelo,
    string Cor,
    string CPFHospede,
    string? Observacoes
);