using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Comands;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Handlers
{
    public class RegistrarUsuarioCommandHandler(
        UserManager<Usuario> userManager,
        ITokenProvider tokenProvider,
        IRefreshTokenProvider refreshTokenProvider 
    ) : IRequestHandler<RegistrarUsuarioCommand, Result<AccessToken>>
    {
        public async Task<Result<AccessToken>> Handle(RegistrarUsuarioCommand command, CancellationToken cancellationToken)
        {
            if (!command.Senha.Equals(command.ConfirmarSenha))
                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("A confirmação de senha falhou."));

            var usuario = new Usuario
            {
                FullName = command.NomeCompleto,
                UserName = command.Email,
                Email = command.Email
            };

            var usuarioResult = await userManager.CreateAsync(usuario, command.Senha);

            if (!usuarioResult.Succeeded)
            {
                var erros = usuarioResult.Errors.Select(err =>
                {
                    return err.Code switch
                    {
                        "DuplicateUserName" => "Já existe um usuário com esse nome.",
                        "DuplicateEmail" => "Já existe um usuário com esse e-mail.",
                        "PasswordTooShort" => "A senha é muito curta.",
                        "PasswordRequiresNonAlphanumeric" => "A senha deve conter pelo menos um caractere especial.",
                        "PasswordRequiresDigit" => "A senha deve conter pelo menos um número.",
                        "PasswordRequiresUpper" => "A senha deve conter pelo menos uma letra maiúscula.",
                        "PasswordRequiresLower" => "A senha deve conter pelo menos uma letra minúscula.",
                        _ => err.Description
                    };
                });

                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
            }

            var tokenAcesso = tokenProvider.GerarAccessToken(usuario);

            var refreshToken = refreshTokenProvider.GerarRefreshToken(usuario);

            await refreshTokenProvider.SalvarRefreshTokenAsync(refreshToken);

            return Result.Ok(tokenAcesso);
        }
    }
}
