using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloTicket;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.AutoMapper;

public class TicketModelsMappingProfile : Profile
{
    public TicketModelsMappingProfile()
    {
        CreateMap<CadastrarTicketRequest, CadastrarTicketCommand>();
        CreateMap<CadastrarTicketResult, CadastrarTicketResponse>();

        CreateMap<(Guid id, EditarTicketRequest request), EditarTicketCommand>()
            .ConvertUsing(src => new EditarTicketCommand(
                src.id,
                src.request.PlacaVeiculo,
                src.request.Ativo
            ));

        CreateMap<EditarTicketResult, EditarTicketResponse>()
            .ConvertUsing(src => new EditarTicketResponse(
                src.Id,
                src.PlacaVeiculo,
                src.NumeroTicket,
                src.Ativo
            ));

        CreateMap<Guid, ExcluirTicketCommand>()
            .ConstructUsing(src => new ExcluirTicketCommand(src));

        CreateMap<SelecionarTicketsRequest, SelecionarTicketsQuery>();

        CreateMap<SelecionarTicketsResult, SelecionarTicketsResponse>()
            .ConvertUsing((src, dest, ctx) => new SelecionarTicketsResponse(
                src.Tickets.Count,
                src?.Tickets.Select(t => ctx.Mapper.Map<SelecionarTicketsDto>(t)).ToImmutableList()
                ?? ImmutableList<SelecionarTicketsDto>.Empty
            ));

        CreateMap<SelecionarTicketsItemDto, SelecionarTicketsDto>()
            .ConvertUsing(src => new SelecionarTicketsDto(
                src.Id,
                src.PlacaVeiculo,
                src.NumeroTicket,
                src.DataCriacao,
                src.Ativo
            ));

        CreateMap<Guid, SelecionarTicketPorIdQuery>()
            .ConvertUsing(src => new SelecionarTicketPorIdQuery(src));

        CreateMap<SelecionarTicketPorIdResult, SelecionarTicketPorIdResponse>()
            .ConvertUsing(src => new SelecionarTicketPorIdResponse(
                src.Id,
                src.PlacaVeiculo,
                src.NumeroTicket,
                src.DataCriacao,
                src.Ativo
            ));

        CreateMap<ObterTicketPorNumeroRequest, ObterTicketPorNumeroQuery>();
        CreateMap<ObterTicketsAtivosRequest, ObterTicketsAtivosQuery>();
        CreateMap<ObterTicketsPorVeiculoRequest, ObterTicketsPorVeiculoQuery>()
            .ConvertUsing(src => new ObterTicketsPorVeiculoQuery(src.PlacaVeiculo));
    }
}