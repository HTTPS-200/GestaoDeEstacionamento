using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloGestaoDeVagas;
public class MapeadorVaga : IEntityTypeConfiguration<Vaga>
{
    public void Configure(EntityTypeBuilder<Vaga> builder)
    {
        {
            builder.ToTable("vagas");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Zona)
                .IsRequired()
                .HasMaxLength(20);

            builder.Ignore(v => v.Ocupada);

            builder.HasOne(v => v.VeiculoEstacionado)
                .WithMany()
                .HasForeignKey("VeiculoEstacionadoId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
