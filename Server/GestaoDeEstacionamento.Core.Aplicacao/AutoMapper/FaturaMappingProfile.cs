using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFatura.Mapping;

public class FaturaMappingProfile : Profile
{
    public FaturaMappingProfile()
    {
        // Command para Domain
        CreateMap<CriarFaturaCommand, Fatura>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Pago, opt => opt.Ignore())
            .ForMember(dest => dest.DataPagamento, opt => opt.Ignore())
            .ForMember(dest => dest.UsuarioId, opt => opt.Ignore());

        // Domain para Results
        CreateMap<Fatura, CriarFaturaResult>()
            .ConvertUsing((src, dest) => new CriarFaturaResult(
                src.Id,
                src.NumeroTicket,
                src.PlacaVeiculo,
                src.DataHoraSaida,
                src.Diarias,
                src.ValorTotal,
                src.Pago
            ));

        CreateMap<Fatura, MarcarFaturaComoPagaResult>()
            .ConvertUsing((src, dest) => new MarcarFaturaComoPagaResult(
                src.Id,
                src.NumeroTicket,
                src.DataPagamento ?? DateTime.UtcNow,
                src.Pago
            ));

        CreateMap<Fatura, ObterFaturaPorIdResult>()
            .ConvertUsing((src, dest) => new ObterFaturaPorIdResult(
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

        CreateMap<Fatura, FaturaItemResult>()
            .ConvertUsing((src, dest) => new FaturaItemResult(
                src.Id,
                src.NumeroTicket,
                src.PlacaVeiculo,
                src.DataHoraSaida,
                src.Diarias,
                src.ValorTotal,
                src.Pago,
                src.DataPagamento
            ));

        CreateMap<Fatura, FaturaDetalheResult>()
            .ConvertUsing((src, dest) => new FaturaDetalheResult(
                src.DataHoraSaida.Date,
                src.NumeroTicket,
                src.PlacaVeiculo,
                src.Diarias,
                src.ValorTotal
            ));

        // Collections para Results
        CreateMap<IEnumerable<Fatura>, ObterFaturasResult>()
            .ConvertUsing((src, dest, ctx) =>
                new ObterFaturasResult(
                    src?.Select(f => ctx.Mapper.Map<FaturaItemResult>(f)).ToImmutableList()
                    ?? ImmutableList<FaturaItemResult>.Empty
                )
            );

        CreateMap<IEnumerable<Fatura>, ObterRelatorioFaturamentoResult>()
            .ConvertUsing((src, dest, ctx) =>
            {
                var faturasDetalhe = src?.Select(f => ctx.Mapper.Map<FaturaDetalheResult>(f)).ToImmutableList()
                    ?? ImmutableList<FaturaDetalheResult>.Empty;

                var valorTotal = src?.Sum(f => f.ValorTotal) ?? 0;
                var totalDiarias = src?.Sum(f => f.Diarias) ?? 0;
                var totalVeiculos = src?.Select(f => f.VeiculoId).Distinct().Count() ?? 0;

                return new ObterRelatorioFaturamentoResult(
                    src?.Min(f => f.DataHoraSaida) ?? DateTime.MinValue,
                    src?.Max(f => f.DataHoraSaida) ?? DateTime.MaxValue,
                    faturasDetalhe,
                    valorTotal,
                    totalDiarias,
                    totalVeiculos
                );
            });
    }
}