using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloCheckIn;

public class MapeadorRegistroCheckInEmOrm : IEntityTypeConfiguration<RegistroCheckIn>
{
    public void Configure(EntityTypeBuilder<RegistroCheckIn> builder)
    {
        builder.ToTable("TBRegistroCheckIn");

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.VeiculoId)
            .IsRequired();

        builder.Property(x => x.TicketId)
            .IsRequired();

        builder.Property(x => x.DataHoraCheckIn)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.Property(x => x.NumeroTicket)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Ativo)
            .IsRequired();

        builder.HasOne(x => x.Veiculo)
            .WithMany()
            .HasForeignKey(x => x.VeiculoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Ticket)
            .WithMany()
            .HasForeignKey(x => x.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Id)
            .IsUnique();

        builder.HasIndex(x => x.NumeroTicket)
            .IsUnique();

        builder.HasIndex(x => x.VeiculoId);
    }
}