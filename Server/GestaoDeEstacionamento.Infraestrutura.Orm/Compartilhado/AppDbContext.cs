using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado
{
    public class AppDbContext(DbContextOptions options, ITenantProvider? tenantProvider = null) :
    IdentityDbContext<Usuario, Cargo, Guid>(options), IUnitOfWork
    {
        public DbSet<Veiculo> Veiculos => Set<Veiculo>();
        public DbSet<Vaga> Vagas => Set<Vaga>();
        public DbSet<Fatura> Faturas => Set<Fatura>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (tenantProvider is not null)
            {
                modelBuilder.Entity<Veiculo>()
                    .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

                modelBuilder.Entity<Vaga>()
                   .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));

                modelBuilder.Entity<Fatura>()
                    .HasQueryFilter(x => x.UsuarioId.Equals(tenantProvider.UsuarioId));
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

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
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }

            await Task.CompletedTask;
        }
    }

}
