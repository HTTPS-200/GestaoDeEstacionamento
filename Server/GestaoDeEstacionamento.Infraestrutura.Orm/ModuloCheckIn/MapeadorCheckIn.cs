using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloCheckIn;
public class MapeadorCheckIn : IEntityTypeConfiguration<Fatura>
{
    public void Configure(EntityTypeBuilder<Fatura> builder)
    {     
    }
}
