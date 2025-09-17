using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloGestaoDeVagas;
using GestaoDeEstacionamento.Core.Dominio.ModuloRecepcaoChekInVeiculo;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFaturamento;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloGestaoDeVagas;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloRecepcaoCheckInVeiculo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoDeEstacionamento.Infraestrutura.Orm
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCamadaInfraestruturaOrm(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepositorioFaturamento, RepositorioFaturamentoEmOrm>();
            services.AddScoped<IRepositorioVaga, RepositorioVagaEmOrm>();
            services.AddScoped<IRepositorioVeiculo, RepositorioVeiculoEmOrm>();

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