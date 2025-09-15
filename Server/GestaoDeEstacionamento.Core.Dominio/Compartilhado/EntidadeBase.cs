using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.Compartilhado
{
    public abstract class EntidadeBase<T>
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }

        public abstract void AtualizarRegistro(T registroEditado);
    }
}
