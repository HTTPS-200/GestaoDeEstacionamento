using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.AutoMapper;

public class VeiculoModelsMappingProfile : Profile
{
    public VeiculoModelsMappingProfile()
    {
        CreateMap<CadastrarVeiculoRequest, CadastrarVeiculoCommand>();
        CreateMap<CadastrarVeiculoResult, CadastrarVeiculoResponse>();

        CreateMap<(Guid, EditarVeiculoRequest), EditarVeiculoCommand>()
            .ConvertUsing(src => new EditarVeiculoCommand(
                src.Item1,
                src.Item2.Placa,
                src.Item2.Modelo,
                src.Item2.Cor,
                src.Item2.CPFHospede,
                src.Item2.Observacoes
            ));

        CreateMap<EditarVeiculoResult, EditarVeiculoResponse>();

        CreateMap<Guid, ExcluirVeiculoCommand>()
            .ConstructUsing(src => new ExcluirVeiculoCommand(src));

        CreateMap<SelecionarVeiculosRequest, SelecionarVeiculosQuery>();

        CreateMap<SelecionarVeiculosResult, SelecionarVeiculosResponse>()
            .ConvertUsing((src, dest, ctx) => new SelecionarVeiculosResponse(
                src.Veiculos.Count,
                src?.Veiculos.Select(v => ctx.Mapper.Map<SelecionarVeiculosDto>(v)).ToImmutableList() ?? ImmutableList<SelecionarVeiculosDto>.Empty
            ));

        CreateMap<Guid, SelecionarVeiculoPorIdQuery>()
            .ConvertUsing(src => new SelecionarVeiculoPorIdQuery(src));

        CreateMap<SelecionarVeiculoPorIdResult, SelecionarVeiculoPorIdResponse>()
            .ConvertUsing(src => new SelecionarVeiculoPorIdResponse(
                src.Id,
                src.Placa,
                src.Modelo,
                src.Cor,
                src.CPFHospede,
                src.Observacoes,
                src.DataEntrada,
                src.DataSaida
            ));
    }
}