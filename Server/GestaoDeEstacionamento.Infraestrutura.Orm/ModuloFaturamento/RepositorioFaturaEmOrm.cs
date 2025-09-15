using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFaturamento;

public class RepositorioFaturaEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<Fatura>(contexto), IRepositorioFatura;