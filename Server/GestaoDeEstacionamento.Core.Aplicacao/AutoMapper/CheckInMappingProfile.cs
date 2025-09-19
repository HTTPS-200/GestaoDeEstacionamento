using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Comands;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.AutoMapper;

public class CheckInMappingProfile : Profile
{
    public CheckInMappingProfile()
    {
        CreateMap<RealizarCheckInCommand, Veiculo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataEntrada, opt => opt.Ignore())
            .ForMember(dest => dest.DataSaida, opt => opt.Ignore())
            .ForMember(dest => dest.UsuarioId, opt => opt.Ignore());

        CreateMap<RegistroCheckIn, RealizarCheckInResult>()
            .ConvertUsing(src => new RealizarCheckInResult(
                src.Id,
                src.VeiculoId,
                src.TicketId,
                src.NumeroTicket,
                src.DataHoraCheckIn
            ));

        CreateMap<RegistroCheckIn, SelecionarCheckInPorIdResult>()
            .ConvertUsing(src => new SelecionarCheckInPorIdResult(
                src.Id,
                src.VeiculoId,
                src.TicketId,
                src.NumeroTicket,
                src.DataHoraCheckIn,
                src.Ativo
            ));

        CreateMap<RegistroCheckIn, SelecionarCheckInsDto>()
           .ConvertUsing(src => new SelecionarCheckInsDto(
                src.Id,
                src.VeiculoId,
                src.TicketId,
                src.NumeroTicket,
                src.DataHoraCheckIn,
                src.Ativo
            ));

        CreateMap<IEnumerable<RegistroCheckIn>, SelecionarCheckInsResult>()
         .ConvertUsing((src, dest, ctx) =>
             new SelecionarCheckInsResult(
                 src?.Select(c => ctx.Mapper.Map<SelecionarCheckInsDto>(c)).ToImmutableList() ?? ImmutableList<SelecionarCheckInsDto>.Empty
             )
         );
    }
}