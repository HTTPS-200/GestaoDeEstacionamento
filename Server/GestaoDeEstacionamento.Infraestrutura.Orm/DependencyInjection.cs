using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloSaidaLiberacao;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFaturamento;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVaga;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloSaidaLiberacao;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GestaoDeEstacionamento.Core.Dominio.ModuloRelatorio;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloRelatorio;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVeiculo;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloCheckIn;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;

namespace GestaoDeEstacionamento.Infraestrutura.Orm
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCamadaInfraestruturaOrm(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped
            services.AddScoped<IRefreshTokenProvider, RefreshTokenProvider>();
            services.AddScoped<IRepositorioFatura, RepositorioFaturaEmOrm>();
            services.AddScoped<IRepositorioVaga, RepositorioVagaEmOrm>();
            services.AddScoped<IRepositorioSaida, RepositorioSaidaEmOrm>();
            services.AddScoped<IRepositorioTicket, RepositorioTicketEmOrm>();
            services.AddScoped<IRepositorioRelatorioFinanceiro, RepositorioRelatorioFinanceiroEmOrm>();
            services.AddScoped<IRepositorioVeiculo, RepositorioVeiculoEmOrm>();
            services.AddScoped<IRepositorioRegistroCheckIn, RepositorioRegistroCheckInEmOrm>();


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