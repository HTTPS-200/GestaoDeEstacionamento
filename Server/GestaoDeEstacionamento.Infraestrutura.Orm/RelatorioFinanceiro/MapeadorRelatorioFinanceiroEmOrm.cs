using GestaoDeEstacionamento.Core.Dominio.ModuloRelatorio;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloRelatorio;

public class MapeadorRelatorioFinanceiroEmOrm : IEntityTypeConfiguration<RelatorioFinanceiro>
{
    public void Configure(EntityTypeBuilder<RelatorioFinanceiro> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.DataInicio)
            .IsRequired();

        builder.Property(x => x.DataFim)
            .IsRequired();

        builder.HasMany(x => x.Faturas)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "RelatorioFinanceiroFaturas",
                r => r.HasOne<Fatura>().WithMany().HasForeignKey("FaturaId").IsRequired(),
                l => l.HasOne<RelatorioFinanceiro>().WithMany().HasForeignKey("RelatorioFinanceiroId").IsRequired()
            );

        builder.HasIndex(x => x.Id)
            .IsUnique();
    }
}
