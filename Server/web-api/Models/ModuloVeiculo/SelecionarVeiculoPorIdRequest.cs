namespace GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo
{
    public record SelecionarVeiculoPorIdRequest(Guid TicketId);

    public record SelecionarVeiculoPorIdResponse(
        Guid TicketId,
        string Placa,
        string Modelo,
        string Cor,
        string CpfHospede,
        string? Observacoes,
        DateTime DataEntrada
    );
}
