using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Comands;
using GestaoDeEstacionamento.WebApi.Models.ModuloAutenticacao;

namespace GestaoDeEstacionamento.WebApi.AutoMapper
{
    public class AutenticacaoModelsMappingProfile : Profile
    {
        public AutenticacaoModelsMappingProfile()
        {
            CreateMap<RegistrarUsuarioRequest, RegistrarUsuarioCommand>();
            CreateMap<AutenticarUsuarioRequest, AutenticarUsuarioCommand>();
        }
    }
}
