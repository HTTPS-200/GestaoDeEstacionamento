using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloFatura;

public interface IRepositorioFatura : IRepositorio<Fatura>
{
    Task<Fatura?> ObterPorNumeroTicket(string numeroTicket);
    Task<Fatura?> ObterPorPlaca(string placa);
    Task<List<Fatura>> ObterPorPeriodo(DateTime inicio, DateTime fim);
    Task<List<Fatura>> ObterPorVeiculoId(Guid veiculoId);
    Task<List<Fatura>> ObterNaoPagas();
    Task<List<Fatura>> ObterPorPeriodo(DateTime inicio, DateTime fim, Guid usuarioId);
}