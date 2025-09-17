using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao
{
    public class RefreshTokenProvider : IRefreshTokenProvider
    {
        private readonly IConfiguration config;

        public RefreshTokenProvider(IConfiguration config)
        {
            this.config = config;
        }

        public RefreshToken GerarRefreshToken(Usuario usuario)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return new RefreshToken
            {
                UsuarioId = usuario.Id,
                Token = Convert.ToBase64String(randomNumber),
                Expiracao = DateTime.UtcNow.AddDays(7), 
                Usado = false,
                Revogado = false
            };
        }

        public Task SalvarRefreshTokenAsync(RefreshToken token)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken?> ObterRefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task InvalidarRefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}