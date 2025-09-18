using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
public interface IRepositorioVeiculo : IRepositorio<Veiculo>
{
    List<Veiculo> ObterPorPlaca(string placa);
    List<Veiculo> ObterVeiculosEstacionados();
    Veiculo? ObterPorTicket(string numeroTicket);
}