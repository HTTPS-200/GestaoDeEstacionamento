using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
