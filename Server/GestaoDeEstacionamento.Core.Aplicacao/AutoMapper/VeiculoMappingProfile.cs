using AutoMapper;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckIn.Commands;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.AutoMapper;

public class VeiculoMappingProfile : Profile
{
    public VeiculoMappingProfile()
    {
        // Commands -> Entity
        CreateMap<CadastrarVeiculoCommand, Veiculo>();
        CreateMap<EditarVeiculoCommand, Veiculo>();

        // Entity -> Results
        CreateMap<Veiculo, CadastrarVeiculoResult>()
            .ConvertUsing(src => new CadastrarVeiculoResult(
                src.Ticket,
                src.Placa,
                src.Modelo,
                src.Cor,
                src.CpfHospede,
                src.Observacoes,
                src.DataEntrada
            ));

        CreateMap<Veiculo, EditarVeiculoResult>()
            .ConvertUsing(src => new EditarVeiculoResult(
                src.Ticket,
                src.Placa,
                src.Modelo,
                src.Cor,
                src.CpfHospede,
                src.Observacoes,
                src.DataEntrada
            ));

        CreateMap<Veiculo, SelecionarVeiculoPorTicketResult>()
            .ConvertUsing(src => new SelecionarVeiculoPorTicketResult(
                src.Ticket,
                src.Placa,
                src.Modelo,
                src.Cor,
                src.CpfHospede,
                src.Observacoes,
                src.DataEntrada
            ));

        CreateMap<Veiculo, SelecionarVeiculosDto>()
            .ConvertUsing(src => new SelecionarVeiculosDto(
                src.Ticket,
                src.Placa,
                src.Modelo,
                src.Cor,
                src.CpfHospede,
                src.Observacoes,
                src.DataEntrada
            ));

        CreateMap<IEnumerable<Veiculo>, SelecionarVeiculosResult>()
            .ConvertUsing((src, dest, ctx) =>
            {
                var veiculos = src
                    .Select(v => ctx.Mapper.Map<SelecionarVeiculosDto>(v))
                    .ToImmutableList();

                return new SelecionarVeiculosResult(
                    veiculos.Count,
                    veiculos
                );
            });
    }
}
