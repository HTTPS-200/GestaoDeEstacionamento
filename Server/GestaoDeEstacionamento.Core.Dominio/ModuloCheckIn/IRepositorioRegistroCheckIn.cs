using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;

public interface IRepositorioRegistroCheckIn : IRepositorio<RegistroCheckIn>
{
    Task<RegistroCheckIn?> ObterPorNumeroTicket(string numeroTicket);
    Task<RegistroCheckIn?> ObterPorPlacaVeiculo(string placa);
    Task<List<RegistroCheckIn>> ObterCheckInsAtivos();
    Task<List<RegistroCheckIn>> ObterCheckInsPorVeiculoId(Guid veiculoId);
    Task<List<RegistroCheckIn>> ObterTodosAsync();
    Task<RegistroCheckIn?> ObterPorIdAsync(Guid id);

}