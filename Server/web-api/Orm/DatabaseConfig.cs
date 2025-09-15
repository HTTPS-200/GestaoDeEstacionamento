using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.WebApi.Orm
{
    public static class DatabaseOperations
    {
        public static void ApplyMigrations(this IHost app)
        {
            var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Database.Migrate();
        }
    }
}