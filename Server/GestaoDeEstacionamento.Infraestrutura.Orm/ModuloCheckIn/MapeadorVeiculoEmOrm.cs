using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloCheckIn;
public class MapeadorVeiculoEmOrm : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        builder.ToTable("Veiculos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Placa)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(v => v.Modelo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(v => v.Cor)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(v => v.CpfHospede)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(v => v.Observacoes)
            .HasMaxLength(500);

        builder.HasMany(v => v.Tickets)
            .WithOne(t => t.Veiculo)
            .HasForeignKey(t => t.VeiculoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => v.Placa)
            .IsUnique();
    }
}
