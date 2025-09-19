namespace GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo;

public record SelecionarVeiculoPorIdRequest(Guid Id);

public record SelecionarVeiculoPorIdResponse(
    Guid Id,
    string Placa,
    string Modelo,
    string Cor,
    string CPFHospede,
    string? Observacoes,
    DateTime DataEntrada,
    DateTime? DataSaida
);