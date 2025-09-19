using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.AutoMapper;

public class TicketMappingProfile : Profile
{
    public TicketMappingProfile()
    {
        CreateMap<CadastrarTicketCommand, Ticket>()
            .ForMember(dest => dest.VeiculoId, opt => opt.Ignore()) 
            .ForMember(dest => dest.NumeroTicket, opt => opt.Ignore()); 

        CreateMap<Ticket, CadastrarTicketResult>()
            .ConvertUsing((src, dest) => new CadastrarTicketResult(
                src.Id,
                src.NumeroTicket,
                ""
            ));

        CreateMap<EditarTicketCommand, Ticket>()
            .ForMember(dest => dest.VeiculoId, opt => opt.Ignore());

        CreateMap<Ticket, EditarTicketResult>()
            .ConvertUsing((src, dest) => new EditarTicketResult(
                src.Id,
                "", 
                src.NumeroTicket,
                src.Ativo
            ));

        CreateMap<Ticket, SelecionarTicketPorIdResult>()
            .ConvertUsing((src, dest) => new SelecionarTicketPorIdResult(
                src.Id,
                "", 
                src.NumeroTicket,
                src.DataCriacao,
                src.Ativo
            ));

        CreateMap<Ticket, SelecionarTicketsItemDto>()
           .ConvertUsing((src, dest) => new SelecionarTicketsItemDto(
                src.Id,
                "", 
                src.NumeroTicket,
                src.DataCriacao,
                src.Ativo
            ));

        CreateMap<IEnumerable<Ticket>, SelecionarTicketsResult>()
         .ConvertUsing((src, dest, ctx) =>
             new SelecionarTicketsResult(
                 src?.Select(t => ctx.Mapper.Map<SelecionarTicketsItemDto>(t)).ToImmutableList()
                 ?? ImmutableList<SelecionarTicketsItemDto>.Empty
             )
         );
    }
}