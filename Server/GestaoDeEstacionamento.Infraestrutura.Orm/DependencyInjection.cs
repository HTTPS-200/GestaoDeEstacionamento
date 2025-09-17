using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoDeEstacionamento.Infraestrutura.Orm
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCamadaInfraestruturaOrm(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped
            //services.AddScoped<IRepositorioX, RepositorioX>();


            services.AddEntityFrameworkConfig(configuration);

            return services;
        }

        private static void AddEntityFrameworkConfig(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionString = configuration["SQL_CONNECTION_STRING"];

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("A variável SQL_CONNECTION_STRING não foi fornecida.");

            services.AddDbContext<IUnitOfWork, AppDbContext>(options =>
                options.UseNpgsql(connectionString, (opt) => opt.EnableRetryOnFailure(3)));
        }
    }
}