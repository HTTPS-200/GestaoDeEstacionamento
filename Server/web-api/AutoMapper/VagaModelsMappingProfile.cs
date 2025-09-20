using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloVaga;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.AutoMapper;

public class VagaModelsMappingProfile : Profile
{
    public VagaModelsMappingProfile()
    {
        CreateMap<CriarVagaRequest, CriarVagaCommand>();
        CreateMap<CriarVagaResult, CriarVagaResponse>();

        CreateMap<EditarVagaRequest, EditarVagaCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.VeiculoId, opt => opt.Ignore());

        CreateMap<EditarVagaResult, EditarVagaResponse>();
        CreateMap<ObterVagaPorIdResult, ObterVagaPorIdResponse>();
        CreateMap<ObterVagaItemResult, ObterVagaItemResponse>();

        CreateMap<ObterTodasVagasResult, ObterTodasVagasResponse>()
            .ConvertUsing((src, dest, ctx) => new ObterTodasVagasResponse(
                src.Vagas.Select(v => new ObterVagaItemResponse(
                    v.Id,
                    v.Identificador,
                    v.Zona,
                    v.Ocupada,
                    v.VeiculoId
                )).ToImmutableList()
            ));

        CreateMap<OcuparVagaRequest, OcuparVagaCommand>()
            .ForMember(dest => dest.VagaId, opt => opt.Ignore());

        CreateMap<OcuparVagaResult, OcuparVagaResponse>();
        CreateMap<LiberarVagaResult, LiberarVagaResponse>();
    }
}