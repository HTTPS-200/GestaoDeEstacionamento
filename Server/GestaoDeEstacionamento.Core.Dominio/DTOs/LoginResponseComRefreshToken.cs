using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.DTOs
{
    public record LoginResponseComRefreshToken(
        AccessToken AccessToken,
        string RefreshToken
    );
}

