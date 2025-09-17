using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Comands
{
    public record SairCommand : IRequest<Result>;
}
