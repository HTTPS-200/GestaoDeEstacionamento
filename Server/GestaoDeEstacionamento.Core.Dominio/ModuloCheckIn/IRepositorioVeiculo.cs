using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;

public interface IRepositorioVeiculo : IRepositorio<Veiculo>
{
    Task<List<Veiculo>> ObterPorPlaca(string placa);
    Task<List<Veiculo>> ObterVeiculosEstacionados();
    Task<Veiculo?> ObterPorId(Guid id);
}