using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Comands;
using GestaoDeEstacionamento.Core.Dominio.DTOs;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Handlers
{
    public class AutenticarUsuarioCommandHandler(
        SignInManager<Usuario> signInManager,
        UserManager<Usuario> userManager,
        ITokenProvider tokenProvider,
        IRefreshTokenProvider refreshTokenProvider
    ) : IRequestHandler<AutenticarUsuarioCommand, Result<LoginResponseComRefreshToken>>
    {
        public async Task<Result<LoginResponseComRefreshToken>> Handle(
            AutenticarUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            var usuarioEncontrado = await userManager.FindByEmailAsync(request.Email);

            if (usuarioEncontrado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro("Não foi possível encontrar o usuário requisitado."));

            var resultadoLogin = await signInManager.PasswordSignInAsync(
                usuarioEncontrado.UserName!,
                request.Senha,
                isPersistent: true,
                lockoutOnFailure: false
            );

            if (!resultadoLogin.Succeeded)
            {
                if (resultadoLogin.IsLockedOut)
                    return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("Sua conta foi bloqueada temporariamente devido a muitas tentativas inválidas."));

                if (resultadoLogin.IsNotAllowed)
                    return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("Não é permitido efetuar login. Verifique se sua conta está confirmada."));

                if (resultadoLogin.RequiresTwoFactor)
                    return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("É necessário confirmar o login com autenticação de dois fatores."));

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("Login ou senha incorretos."));
            }

            var accessToken = tokenProvider.GerarAccessToken(usuarioEncontrado);

            var refreshToken = refreshTokenProvider.GerarRefreshToken(usuarioEncontrado);
            await refreshTokenProvider.SalvarRefreshTokenAsync(refreshToken);

            var response = new LoginResponseComRefreshToken(
                AccessToken: accessToken,
                RefreshToken: refreshToken.Token
            );

            return Result.Ok(response);
        }
    }
}
