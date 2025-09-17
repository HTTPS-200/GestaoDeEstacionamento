using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFaturamento;
public class MapeadorFatura : IEntityTypeConfiguration<Fatura>
{
    public void Configure(EntityTypeBuilder<Fatura> builder)
    {
        builder.ToTable("faturas");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.TicketId)
            .IsRequired();

        builder.Property(f => f.DataEntrada)
            .IsRequired();

        builder.Property(f => f.DataSaida)
            .IsRequired();

        builder.Property(f => f.Diarias)
            .IsRequired();

        builder.Property(f => f.ValorDiaria)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Ignore(f => f.ValorTotal); // Ignorar propriedade calculada para evitar erros na migrations
    }
}
