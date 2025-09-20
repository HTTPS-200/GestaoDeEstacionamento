using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloCheckOut.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloCheckOut;

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