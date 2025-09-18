using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
public interface IRepositorioVaga : IRepositorio<Vaga>
{
    List<Vaga> ObterPorStatus(StatusVaga status);
    Vaga? ObterPorIdentificador(string identificador);
    List<Vaga> ObterVagasLivres();
    List<Vaga> ObterVagasOcupadas();
}