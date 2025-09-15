using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloCheckIn;
public class RepositorioChekInEmOrm(AppDbContext contexto) : RepositorioBaseEmOrm<Ticket>(contexto), IRepositorioTicket; 

