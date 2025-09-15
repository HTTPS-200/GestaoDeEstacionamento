using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloCheckIn;
public class MapeadorCheckIn : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Numero)
            .IsRequired();

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.DataEntrada)
            .IsRequired();

        builder.HasOne(t => t.Veiculo)
            .WithMany(v => v.Tickets)
            .HasForeignKey(t => t.VeiculoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.Numero)
            .IsUnique();
    }
}
