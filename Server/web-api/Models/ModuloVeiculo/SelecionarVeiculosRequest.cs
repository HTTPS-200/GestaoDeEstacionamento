using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo;

public record SelecionarVeiculosRequest(int? Quantidade);

public record SelecionarVeiculosResponse(
    int Quantidade,
    ImmutableList<SelecionarVeiculosDto> Veiculos
);