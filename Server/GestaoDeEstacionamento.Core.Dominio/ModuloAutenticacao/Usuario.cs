using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao
{
    public class Usuario : IdentityUser<Guid>
    {
        public string FullName { get; set; }

        public Usuario()
        {
            Id = Guid.NewGuid();
            EmailConfirmed = true;
        }
    }

    public class Cargo : IdentityRole<Guid>;

    public record UsuarioAutenticado(
        Guid Id,
        string NomeCompleto,
        string Email
    );
}
