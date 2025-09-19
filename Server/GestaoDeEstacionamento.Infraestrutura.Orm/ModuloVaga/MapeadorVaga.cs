using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
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
            builder.Property(x => x.Id)
             .ValueGeneratedNever()
             .IsRequired();

            builder.Property(x => x.Zona)
                .IsRequired();

            builder.HasIndex(x => x.Id)
                .IsUnique();
        }
    }
}
