using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn
{
    public class Movimentacao
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public Guid VagaId { get; set; }
        public Guid UsuarioId { get; set; }

        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }

        // Navegação
        public Ticket Ticket { get; set; }
        public Vaga Vaga { get; set; }
        public Usuario Usuario { get; set; }
    }

}
