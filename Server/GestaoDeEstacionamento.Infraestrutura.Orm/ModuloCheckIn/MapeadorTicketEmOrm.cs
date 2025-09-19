using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloTicket;

public class MapeadorTicketEmOrm : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.NumeroTicket)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.VeiculoId)
            .IsRequired();

        builder.Property(x => x.DataCriacao)
            .IsRequired();

        builder.Property(x => x.Ativo)
            .IsRequired();

        builder.OwnsOne(x => x.SequencialInfo, sequencial =>
        {
            sequencial.Property(s => s.UltimoNumero)
                .IsRequired()
                .HasColumnName("UltimoNumeroSequencial");

            sequencial.Property(s => s.DataAtualizacao)
                .IsRequired()
                .HasColumnName("DataAtualizacaoSequencial");
        });

        builder.HasIndex(x => x.Id)
            .IsUnique();

        builder.HasIndex(x => x.NumeroTicket)
            .IsUnique();

        builder.HasIndex(x => x.VeiculoId);
    }
}