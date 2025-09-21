using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFaturamento
{
    public class MapeadorFaturamentoEmOrm : IEntityTypeConfiguration<Fatura>
    {
        public void Configure(EntityTypeBuilder<Fatura> builder)
        {
            builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

            builder.Property(x => x.CheckInId)
                .IsRequired();

            builder.Property(x => x.VeiculoId)
                .IsRequired();

            builder.Property(x => x.TicketId)
                .IsRequired();

            builder.Property(x => x.NumeroTicket)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.PlacaVeiculo)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.ModeloVeiculo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.CorVeiculo)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.CPFHospede)
                .IsRequired()
                .HasMaxLength(14);

            builder.Property(x => x.IdentificadorVaga)
                .HasMaxLength(20);

            builder.Property(x => x.ZonaVaga)
                .HasMaxLength(50);

            builder.Property(x => x.DataHoraEntrada)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            builder.Property(x => x.DataHoraSaida)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            builder.Property(x => x.Diarias)
                .IsRequired();

            builder.Property(x => x.ValorDiaria)
                .IsRequired()
                .HasColumnType("numeric(18,2)");

            builder.Property(x => x.ValorTotal)
                .IsRequired()
                .HasColumnType("numeric(18,2)");

            builder.Property(x => x.Pago)
                .IsRequired();

            builder.Property(x => x.DataPagamento)
                .HasColumnType("timestamp with time zone");

            builder.Property(x => x.UsuarioId)
                .IsRequired();

            builder.HasIndex(x => x.Id)
                .IsUnique();

            builder.HasIndex(x => x.NumeroTicket)
                .IsUnique();

            builder.HasIndex(x => x.PlacaVeiculo);

            builder.HasIndex(x => x.DataHoraSaida);

            builder.HasIndex(x => x.Pago);

            builder.HasIndex(x => x.VeiculoId);

            builder.HasIndex(x => x.TicketId);
        }
    }
}
