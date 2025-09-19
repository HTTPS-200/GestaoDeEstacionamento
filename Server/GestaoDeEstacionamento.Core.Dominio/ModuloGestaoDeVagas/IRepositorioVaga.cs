using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloVaga;

public interface IRepositorioVaga : IRepositorio<Vaga>
{
    Task<List<Vaga>> ObterVagasLivres();
    Task<List<Vaga>> ObterVagasOcupadas();
    Task<Vaga?> ObterPorIdentificador(string identificador);
    Task<Vaga?> ObterPorVeiculoId(Guid veiculoId);
    Task<bool> VerificarDisponibilidade(string identificador);
}