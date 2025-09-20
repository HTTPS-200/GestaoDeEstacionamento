using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloDeRegistroCheckIn.Comands;
using GestaoDeEstacionamento.WebApi.Models.ModuloRegistroCheckIn;

namespace GestaoDeEstacionamento.WebApi.AutoMapper;

public class CheckOutModelsMappingProfile : Profile
{
    public CheckOutModelsMappingProfile()
    {
        CreateMap<RealizarCheckOutRequest, RealizarCheckOutCommand>();
        CreateMap<RealizarCheckOutResult, RealizarCheckOutResponse>();
        CreateMap<VagaInfoResult, VagaInfoResponse>();
    }
}
