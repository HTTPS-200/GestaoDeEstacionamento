using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao
{
    public class RefreshToken 
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public string Token { get; set; } = string.Empty;
        public Guid UsuarioId { get; set; }
        public DateTime Expiracao { get; set; }
        public bool Usado { get; set; } = false;
        public bool Revogado { get; set; } = false;

        public virtual Usuario Usuario { get; set; } = null!;
    }
}
