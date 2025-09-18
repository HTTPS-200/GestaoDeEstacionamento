using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVeiculo;

public class MapeadorVeiculoEmOrm : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Placa)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.Modelo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Cor)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.CPFHospede)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(x => x.Observacoes)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(x => x.DataEntrada)
            .IsRequired();

        builder.Property(x => x.DataSaida)
            .IsRequired(false);

        builder.HasMany(x => x.Tickets)
            .WithOne(t => t.Veiculo)
            .HasForeignKey(t => t.VeiculoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Id)
            .IsUnique();

        builder.HasIndex(x => x.Placa);
    }
}