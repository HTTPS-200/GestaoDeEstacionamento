using GestaoDeEstacionamento.Core.Aplicacao.ModuloGestaoDeVagas.Command;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloGestaoDeVagas;

public record SelecionarVagasRequest(int? Quantidade);

public record SelecionarVagasResponse(
    int Quantidade,
    ImmutableList<SelecionarVagasDto> Contatos
);
