using GestaoDeEstacionamento.Core.Dominio.ModuloRecepcaoChekInVeiculo;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloRecepcaoCheckInVeiculo;
public class RepositorioVeiculoEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<Veiculo>(contexto), IRepositorioVeiculo;
