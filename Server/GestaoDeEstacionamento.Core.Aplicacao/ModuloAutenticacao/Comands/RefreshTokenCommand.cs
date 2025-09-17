using FluentResults;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Comands
{
    public record RefreshTokenCommand(string Token) : IRequest<Result<AccessToken>>;
}
