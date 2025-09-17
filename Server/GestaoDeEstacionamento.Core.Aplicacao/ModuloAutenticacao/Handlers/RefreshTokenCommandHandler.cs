using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Comands;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Handlers
{
    public class RefreshTokenCommandHandler(
        IRefreshTokenProvider refreshTokenProvider,
        UserManager<Usuario> userManager,
        ITokenProvider tokenProvider
    ) : IRequestHandler<RefreshTokenCommand, Result<AccessToken>>
    {
        public async Task<Result<AccessToken>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await refreshTokenProvider.ObterRefreshTokenAsync(request.Token);

            if (refreshToken is null || refreshToken.Expiracao <= DateTime.UtcNow || refreshToken.Revogado || refreshToken.Usado)
                return Result.Fail("Refresh token inválido ou expirado.");

            var usuario = await userManager.FindByIdAsync(refreshToken.UsuarioId.ToString());
            if (usuario is null)
                return Result.Fail("Usuário não encontrado.");

            var novoAccessToken = tokenProvider.GerarAccessToken(usuario);

            var novoRefreshToken = refreshTokenProvider.GerarRefreshToken(usuario);
            await refreshTokenProvider.SalvarRefreshTokenAsync(novoRefreshToken);

            await refreshTokenProvider.InvalidarRefreshTokenAsync(refreshToken.Token);

            return Result.Ok(novoAccessToken);
        }
    }
}
