using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloAutenticacao
{
    public class MapeadorRefreshToken : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("TB_RefreshToken");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(rt => rt.Expiracao)
                .IsRequired();

            builder.HasOne(rt => rt.Usuario)
                 .WithMany(u => u.RefreshTokens)
                 .HasForeignKey(rt => rt.UsuarioId)
                 .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
