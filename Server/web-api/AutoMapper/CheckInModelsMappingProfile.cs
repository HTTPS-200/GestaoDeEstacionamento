using AutoMapper;
using CoreCheckIn = GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Comands;
using WebCheckIn = GestaoDeEstacionamento.WebApi.Models.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;
using System.Collections.Immutable;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Comands;
using GestaoDeEstacionamento.WebApi.Models.ModuloCheckIn;

namespace GestaoDeEstacionamento.WebApi.AutoMapper;

public class CheckInModelsMappingProfile : Profile
{
    public CheckInModelsMappingProfile()
    {
        CreateMap<RealizarCheckInRequest, RealizarCheckInCommand>();
        CreateMap<RealizarCheckInResult, RealizarCheckInResponse>();

        CreateMap<SelecionarCheckInsRequest, SelecionarCheckInsQuery>();

        CreateMap<CoreCheckIn.SelecionarCheckInsDto, WebCheckIn.SelecionarCheckInsDto>();

        CreateMap<SelecionarCheckInsResult, SelecionarCheckInsResponse>()
            .ConvertUsing((src, dest, ctx) => new SelecionarCheckInsResponse(
                src.CheckIns.Count,
                src?.CheckIns.Select(c => ctx.Mapper.Map<WebCheckIn.SelecionarCheckInsDto>(c)).ToImmutableList()
                    ?? ImmutableList<WebCheckIn.SelecionarCheckInsDto>.Empty
            ));

        CreateMap<Guid, SelecionarCheckInPorIdQuery>()
            .ConvertUsing(src => new SelecionarCheckInPorIdQuery(src));

        CreateMap<SelecionarCheckInPorIdResult, SelecionarCheckInPorIdResponse>()
            .ConvertUsing(src => new SelecionarCheckInPorIdResponse(
                src.Id,
                src.VeiculoId,
                src.TicketId,
                src.NumeroTicket,
                src.DataHoraCheckIn,
                src.Ativo
            ));
    }
}
