using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GestaoDeEstacionamento.Core.Dominio.ModuloRelatorio;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using Microsoft.EntityFrameworkCore.Infrastructure;
using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado
{
    public class AppDbContext(DbContextOptions options, ITenantProvider? tenantProvider = null) :
    IdentityDbContext<Usuario, Cargo, Guid>(options), IUnitOfWork
    {
        // add public DbSet para módulos adicionados

        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<Fatura> Fatura { get; set; }
        public DbSet<RelatorioFinanceiro> RelatorioFinanceiro { get; set; }
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<RegistroCheckIn> RegistrosCheckIn { get; set; } = null!;
        public DbSet<Vaga> Vaga { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (tenantProvider is not null)
            {
                modelBuilder.Entity<Fatura>()
                .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

                modelBuilder.Entity<Vaga>()
               .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

                modelBuilder.Entity<Ticket>()
               .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

                modelBuilder.Entity<RegistroCheckIn>()
                    .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

                modelBuilder.Entity<RelatorioFinanceiro>()
                     .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

                modelBuilder.Entity<Veiculo>()
             .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));


            }

            var assembly = typeof(AppDbContext).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }


        public async Task CommitAsync()
        {
            await SaveChangesAsync();
        }

        public async Task RollbackAsync()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }

            await Task.CompletedTask;
        }

        public DatabaseFacade GetDatabase()
        {
            throw new NotImplementedException();
        }
    }

}
