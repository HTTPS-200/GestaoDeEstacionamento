using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.AutoMapper;

public class VagaMappingProfile : Profile
{
    public VagaMappingProfile()
    {
       CreateMap<CriarVagaCommand, Vaga>()
            .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId));

        CreateMap<Vaga, CriarVagaResult>();
        CreateMap<Vaga, EditarVagaResult>();
        CreateMap<Vaga, ObterVagaPorIdResult>();
        CreateMap<Vaga, ObterVagaItemResult>();

        CreateMap<IEnumerable<Vaga>, ObterTodasVagasResult>()
            .ConvertUsing((src, dest, ctx) =>
                new ObterTodasVagasResult(
                    src?.Select(v => new ObterVagaItemResult(
                        v.Id,
                        v.Identificador,
                        v.Zona,
                        v.Ocupada,
                        v.VeiculoId
                    )).ToImmutableList()
                    ?? ImmutableList<ObterVagaItemResult>.Empty
                )
            );
    }
}