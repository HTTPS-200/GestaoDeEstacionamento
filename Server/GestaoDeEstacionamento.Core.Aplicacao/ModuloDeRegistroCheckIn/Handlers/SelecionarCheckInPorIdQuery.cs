using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Comands;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Handlers
{
    public class SelecionarCheckInPorIdQueryHandler :
     IRequestHandler<SelecionarCheckInPorIdQuery, Result<SelecionarCheckInPorIdResult>>
    {
        private readonly IRepositorioRegistroCheckIn _repositorioCheckIn;
        private readonly IMapper _mapper;

        public SelecionarCheckInPorIdQueryHandler(
            IRepositorioRegistroCheckIn repositorioCheckIn,
            IMapper mapper)
        {
            _repositorioCheckIn = repositorioCheckIn;
            _mapper = mapper;
        }

        public async Task<Result<SelecionarCheckInPorIdResult>> Handle(
            SelecionarCheckInPorIdQuery query, CancellationToken cancellationToken)
        {
            var checkIn = await _repositorioCheckIn.ObterPorIdAsync(query.Id);

            if (checkIn == null)
            {
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(
                    $"Check-in com ID {query.Id} não encontrado"));
            }

            var result = new SelecionarCheckInPorIdResult(
                checkIn.Id,
                checkIn.VeiculoId,
                checkIn.TicketId,
                checkIn.NumeroTicket,
                checkIn.DataHoraCheckIn,
                checkIn.Ativo
            );

            return Result.Ok(result);
        }
    }
}
