using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloCheckIn;
public class RepositorioVeiculoEmOrm(AppDbContext contexto) : RepositorioBaseEmOrm<Veiculo>(contexto), IRepositorioVeiculo;
