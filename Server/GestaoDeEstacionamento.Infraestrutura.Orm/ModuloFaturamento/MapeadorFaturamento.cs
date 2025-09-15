using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFaturamento
{
    public class MapeadorFaturamentoEmOrm : IEntityTypeConfiguration<Fatura>
    {
        public void Configure(EntityTypeBuilder<Fatura> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.DataEntrada)
                .IsRequired();

            builder.Property(x => x.DataSaida)
                .IsRequired();

            builder.Property(x => x.NumeroDiarias)
                .IsRequired();

            builder.Property(x => x.ValorTotal)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(x => x.TicketId)
                .WithMany()
                .HasForeignKey("TicketId") 
                .IsRequired();

            builder.HasIndex(x => x.Id)
                .IsUnique();
        }
    }
}
