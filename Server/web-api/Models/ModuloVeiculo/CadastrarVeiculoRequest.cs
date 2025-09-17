namespace GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo
{
    public record CadastrarVeiculoRequest(
        string Placa,
        string Modelo,
        string Cor,
        string CpfHospede,
        string? Observacoes = null
    );

    public record CadastrarVeiculoResponse(
        Guid TicketId,
        string Placa,
        string Modelo,
        string Cor,
        string CpfHospede,
        string? Observacoes,
        DateTime DataEntrada
    );
}
