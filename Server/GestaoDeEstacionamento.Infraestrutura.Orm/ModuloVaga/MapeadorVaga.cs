using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVaga
{
    public class MapeadorVaga : IEntityTypeConfiguration<Vaga>
    {
        public void Configure(EntityTypeBuilder<Vaga> builder)
        {
            builder.Property(v => v.Identificador)
           .IsRequired()
           .HasMaxLength(20);

            builder.Property(v => v.Zona)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.Ocupada)
                .IsRequired();

            builder.Property(v => v.VeiculoId)
                .IsRequired(false);

            builder.Property(v => v.UsuarioId)
                .IsRequired();
        }
    }
}
