using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloFatura;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloFatura;

public class FaturaModelsMappingProfile : Profile
{
    public FaturaModelsMappingProfile()
    {
        CreateMap<CriarFaturaResult, CriarFaturaResponse>()
            .ConvertUsing((src, dest) => new CriarFaturaResponse(
                src.Id,
                src.NumeroTicket,
                src.PlacaVeiculo,
                src.DataHoraSaida,
                src.Diarias,
                src.ValorTotal,
                src.Pago
            ));

        CreateMap<MarcarFaturaComoPagaResult, MarcarFaturaComoPagaResponse>()
            .ConvertUsing((src, dest) => new MarcarFaturaComoPagaResponse(
                src.Id,
                src.NumeroTicket,
                src.DataPagamento,
                src.Pago
            ));

        CreateMap<ObterFaturaPorIdResult, ObterFaturaResponse>()
            .ConvertUsing((src, dest) => new ObterFaturaResponse(
                src.Id,
                src.CheckInId,
                src.VeiculoId,
                src.TicketId,
                src.NumeroTicket,
                src.PlacaVeiculo,
                src.ModeloVeiculo,
                src.CorVeiculo,
                src.CPFHospede,
                src.IdentificadorVaga,
                src.ZonaVaga,
                src.DataHoraEntrada,
                src.DataHoraSaida,
                src.Diarias,
                src.ValorDiaria,
                src.ValorTotal,
                src.Pago,
                src.DataPagamento
            ));

        CreateMap<FaturaItemResult, FaturaItemResponse>()
            .ConvertUsing((src, dest) => new FaturaItemResponse(
                src.Id,
                src.NumeroTicket,
                src.PlacaVeiculo,
                src.DataHoraSaida,
                src.Diarias,
                src.ValorTotal,
                src.Pago,
                src.DataPagamento
            ));

        CreateMap<FaturaDetalheResult, FaturaDetalheResponse>()
            .ConvertUsing((src, dest) => new FaturaDetalheResponse(
                src.Data,
                src.NumeroTicket,
                src.Placa,
                src.Diarias,
                src.Valor
            ));

        CreateMap<ObterFaturasResult, ObterFaturasResponse>()
            .ConvertUsing((src, dest, ctx) =>
                new ObterFaturasResponse(
                    src.Faturas?.Select(f => ctx.Mapper.Map<FaturaItemResponse>(f)).ToImmutableList()
                    ?? ImmutableList<FaturaItemResponse>.Empty
                )
            );

        CreateMap<ObterRelatorioFaturamentoResult, ObterRelatorioFaturamentoResponse>()
            .ConvertUsing((src, dest, ctx) =>
                new ObterRelatorioFaturamentoResponse(
                    src.PeriodoInicio,
                    src.PeriodoFim,
                    src.Faturas?.Select(f => ctx.Mapper.Map<FaturaDetalheResponse>(f)).ToImmutableList()
                        ?? ImmutableList<FaturaDetalheResponse>.Empty,
                    src.ValorTotalPeriodo,
                    src.TotalDiarias,
                    src.TotalVeiculos
                )
            );

        CreateMap<CriarFaturaRequest, CriarFaturaCommand>()
            .ConvertUsing((src, dest) => new CriarFaturaCommand(
                src.CheckInId,
                src.VeiculoId,
                src.TicketId,
                src.NumeroTicket,
                src.PlacaVeiculo,
                src.ModeloVeiculo,
                src.CorVeiculo,
                src.CPFHospede,
                src.IdentificadorVaga,
                src.ZonaVaga,
                src.DataHoraEntrada,
                src.DataHoraSaida,
                src.Diarias,
                src.ValorDiaria,
                src.ValorTotal
            ));

        CreateMap<MarcarFaturaComoPagaRequest, MarcarFaturaComoPagaCommand>()
            .ConvertUsing((src, dest) => new MarcarFaturaComoPagaCommand(
                Guid.Empty 
            ));
    }
}