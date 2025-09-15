using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVaga
{
    class RepositorioVagaEmOrm(AppDbContext contexto)
        : RepositorioBaseEmOrm<Vaga>(contexto), IRepositorioVaga;
}
