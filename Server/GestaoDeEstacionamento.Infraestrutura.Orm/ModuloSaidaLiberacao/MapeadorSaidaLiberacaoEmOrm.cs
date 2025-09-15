using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloSaidaLiberacao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloSaidaLiberacao;
public class MapeadorSaidaLiberacaoEmOrm : IEntityTypeConfiguration<Saida>
{
    public void Configure(EntityTypeBuilder<Saida> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.DataSaida)
            .IsRequired();

        builder.HasOne(s => s.TicketId)
            .WithMany()
            .HasForeignKey(s => s.TicketId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
