using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Comands;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Handlers
{
    public class SairCommandHandler(
        SignInManager<Usuario> signInManager
    ) : IRequestHandler<SairCommand, Result>
    {
        public async Task<Result> Handle(SairCommand request, CancellationToken cancellationToken)
        {
            await signInManager.SignOutAsync();

            return Result.Ok();
        }
    }
}
