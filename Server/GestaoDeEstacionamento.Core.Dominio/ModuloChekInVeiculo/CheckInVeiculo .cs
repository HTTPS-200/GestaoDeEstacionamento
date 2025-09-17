using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloChekInVeiculo;
public class CheckInVeiculo : EntidadeBase<CheckInVeiculo>
{
    public Guid VeiculoId { get; set; }
    public Guid FuncionarioId { get; set; }
    public DateTime HorarioCheckIn { get; set; }
    public string? Observacoes { get; set; }

    public CheckInVeiculo(Guid veiculoId, Guid funcionarioId, string? observacoes = null)
    {
        VeiculoId = veiculoId;
        FuncionarioId = funcionarioId;
        HorarioCheckIn = DateTime.UtcNow;
        Observacoes = observacoes;
    }

    public override void AtualizarRegistro(CheckInVeiculo registro)
    {
        if (registro is CheckInVeiculo atualizacao)
        {
            FuncionarioId = atualizacao.FuncionarioId != Guid.Empty ? atualizacao.FuncionarioId : FuncionarioId;
            HorarioCheckIn = atualizacao.HorarioCheckIn != default ? atualizacao.HorarioCheckIn : HorarioCheckIn;
            Observacoes = !string.IsNullOrWhiteSpace(atualizacao.Observacoes) ? atualizacao.Observacoes : Observacoes;
        }
    }
}
