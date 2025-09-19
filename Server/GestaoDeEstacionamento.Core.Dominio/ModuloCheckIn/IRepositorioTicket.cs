using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloTicket;

public interface IRepositorioTicket : IRepositorio<Ticket>
{
    Task<Ticket?> ObterPorNumero(string numeroTicket);
    Task<List<Ticket>> ObterTicketsAtivos();
    Task<List<Ticket>> ObterPorVeiculoId(Guid veiculoId);
    Task<int> ObterUltimoNumeroSequencial();
    Task AtualizarUltimoNumeroSequencial(int ultimoNumero);
    Task<Ticket?> SelecionarRegistroPorIdAsync(Guid idRegistro);
    Task<int> ObterMaiorNumeroSequencial();
}