using GestaoDeEstacionamento.Core.Dominio.ModuloSaidaLiberacao;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloSaidaLiberacao;
public class RepositorioSaidaLiberacaoEmOrm(AppDbContext contexto) : RepositorioBaseEmOrm<Saida>(contexto), IRepositorioSaida;
