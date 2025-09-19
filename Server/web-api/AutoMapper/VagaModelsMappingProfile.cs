using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloVaga;

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
            .ForMember(dest => dest.Vagas, opt => opt.MapFrom(src => src.Vagas));

        CreateMap<OcuparVagaRequest, OcuparVagaCommand>()
            .ForMember(dest => dest.VagaId, opt => opt.Ignore());

        CreateMap<OcuparVagaResult, OcuparVagaResponse>();
        CreateMap<LiberarVagaResult, LiberarVagaResponse>();
    }
}