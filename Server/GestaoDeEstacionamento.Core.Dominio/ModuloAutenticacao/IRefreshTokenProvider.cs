using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao
{
    public interface IRefreshTokenProvider
    {
        RefreshToken GerarRefreshToken(Usuario usuario);
        Task SalvarRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken?> ObterRefreshTokenAsync(string token);
        Task InvalidarRefreshTokenAsync(string token);
    }
}
