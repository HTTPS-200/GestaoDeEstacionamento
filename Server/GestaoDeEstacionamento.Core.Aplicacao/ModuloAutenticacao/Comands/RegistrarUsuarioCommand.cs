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
    public record RegistrarUsuarioCommand(string NomeCompleto, string Email, string Senha, string ConfirmarSenha)
        : IRequest<Result<AccessToken>>;
}
