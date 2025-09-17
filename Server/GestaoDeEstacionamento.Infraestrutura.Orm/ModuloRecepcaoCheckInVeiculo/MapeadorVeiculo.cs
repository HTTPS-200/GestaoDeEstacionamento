using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloRecepcaoCheckInVeiculo;
public class MapeadorVeiculo : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        builder.ToTable("veiculos");

        builder.HasKey(v => v.TicketId);

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
            .HasMaxLength(11);

        builder.Property(v => v.DataEntrada)
            .IsRequired();

        builder.Property(v => v.Observacoes)
            .HasMaxLength(255);

        builder.Property(v => v.DataSaida);
    }
}
