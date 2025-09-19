using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVaga
{
    class RepositorioVagaEmOrm(AppDbContext contexto)
        : RepositorioBaseEmOrm<Vaga>(contexto), IRepositorioVaga
    {
        public Vaga? ObterPorIdentificador(string identificador)
        {
            throw new NotImplementedException();
        }

        public List<Vaga> ObterPorStatus(StatusVaga status)
        {
            throw new NotImplementedException();
        }

        public List<Vaga> ObterVagasLivres()
        {
            throw new NotImplementedException();
        }

        public List<Vaga> ObterVagasOcupadas()
        {
            throw new NotImplementedException();
        }
    }
}
