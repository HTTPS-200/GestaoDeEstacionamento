using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado
{
    public static class AppDbContextFactory
    {
        public static AppDbContext CriarDbContext(string connectionString)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            var dbContext = new AppDbContext(options);

            return dbContext;
        }
    }
}
