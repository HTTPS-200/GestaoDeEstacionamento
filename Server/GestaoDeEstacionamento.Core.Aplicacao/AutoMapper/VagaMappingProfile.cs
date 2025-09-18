using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Command;
using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.AutoMapper;
public class VagaMappingProfile : Profile
{
    public  VagaMappingProfile()
    {
        CreateMap<CadastrarVagaCommand, Vaga>();
        CreateMap<Vaga, CadastrarVagaResult>();

        CreateMap<IEnumerable<Vaga>, SelecionarVagasResult>()
           .ConvertUsing((src, dest, ctx) =>
               new SelecionarVagasResult(src?.Select(v => ctx.Mapper.Map<SelecionarVagasDto>(v))
                   .ToImmutableList() ?? ImmutableList<SelecionarVagasDto>.Empty
               )
           );
    }
}
