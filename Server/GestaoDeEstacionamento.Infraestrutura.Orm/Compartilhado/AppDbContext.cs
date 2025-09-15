using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado
{
    public class AppDbContext(DbContextOptions options, ITenantProvider? tenantProvider = null) :
    IdentityDbContext<Usuario, Cargo, Guid>(options), IUnitOfWork
    {
        // add public DbSet para módulos adicionados

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (tenantProvider is not null)
            { 
                // add modelBuilder para módulos (query filter UsuarioId)
            
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
    }

}
