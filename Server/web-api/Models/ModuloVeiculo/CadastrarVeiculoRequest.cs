namespace GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo;

public record CadastrarVeiculoRequest(
    string Placa,
    string Modelo,
    string Cor,
    string CPFHospede,
    string? Observacoes = null
);

public record CadastrarVeiculoResponse(Guid Id);