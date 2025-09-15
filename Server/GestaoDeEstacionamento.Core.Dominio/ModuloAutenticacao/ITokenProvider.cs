using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao
{
    public interface ITokenProvider
    {
        AccessToken GerarAccessToken(Usuario usuario);
    }

    public record AccessToken(
        string Chave,
        DateTime Expiracao,
        UsuarioAutenticado UsuarioAutenticado
    );
}
