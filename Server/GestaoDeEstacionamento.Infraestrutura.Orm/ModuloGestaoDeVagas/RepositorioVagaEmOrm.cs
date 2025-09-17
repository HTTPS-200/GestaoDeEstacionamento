using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloGestaoDeVagas;
public class RepositorioVagaEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<Vaga>(contexto), IRepositorioVaga;