using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo;
using CoreSelecionarVeiculosDto = GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands.SelecionarVeiculosDto;
using WebSelecionarVeiculosDto = GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo.SelecionarVeiculosDto;


using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.AutoMapper;

public class VeiculoModelsMappingProfile : Profile
{
    public VeiculoModelsMappingProfile()
    {
        CreateMap<CadastrarVeiculoRequest, CadastrarVeiculoCommand>();

        CreateMap<EditarVeiculoRequest, EditarVeiculoCommand>()
     .ForMember(dest => dest.Ticket, opt => opt.Ignore())
     .ForMember(dest => dest.DataEntrada, opt => opt.Ignore());

        CreateMap<SelecionarVeiculosRequest, SelecionarVeiculosQuery>();

        CreateMap<CadastrarVeiculoResult, CadastrarVeiculoResponse>()
            .ConvertUsing(src => new CadastrarVeiculoResponse(
                src.Ticket,
                src.Placa,
                src.Modelo,
                src.Cor,
                src.CpfHospede,
                src.Observacoes,
                src.DataEntrada
            ));

        CreateMap<EditarVeiculoResult, EditarVeiculoResponse>()
            .ConvertUsing(src => new EditarVeiculoResponse(
                src.TicketId,
                src.Placa,
                src.Modelo,
                src.Cor,
                src.CpfHospede,
                src.Observacoes,
                src.DataEntrada
            ));

        CreateMap<SelecionarVeiculoPorTicketResult, SelecionarVeiculoPorIdResponse>()
            .ConvertUsing(src => new SelecionarVeiculoPorIdResponse(
                src.Ticket,
                src.Placa,
                src.Modelo,
                src.Cor,
                src.CpfHospede,
                src.Observacoes,
                src.DataEntrada
            ));

        CreateMap<CoreSelecionarVeiculosDto, WebSelecionarVeiculosDto>()
     .ConvertUsing(src => new WebSelecionarVeiculosDto(
         src.TicketId,
         src.Placa,
         src.Modelo,
         src.Cor,
         src.CpfHospede,
         src.Observacoes,
         src.DataEntrada
     ));

        CreateMap<SelecionarVeiculosResult, SelecionarVeiculosResponse>()
            .ConvertUsing((src, dest, ctx) => new SelecionarVeiculosResponse(
                src.Veiculos.Count,
                src.Veiculos.Select(v => ctx.Mapper.Map<WebSelecionarVeiculosDto>(v)).ToImmutableList()
            ));
    }
}
