using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo
{
    public record SelecionarVeiculosRequest(int? Quantidade);

    public record SelecionarVeiculosDto(
        Guid TicketId,
        string Placa,
        string Modelo,
        string Cor,
        string CpfHospede,
        string? Observacoes,
        DateTime DataEntrada
    );

    public record SelecionarVeiculosResponse(
        int Quantidade,
        ImmutableList<SelecionarVeiculosDto> Veiculos
    );
}
