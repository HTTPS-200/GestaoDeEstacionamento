using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao;

namespace GestaoDeEstacionamento.WebApi.Identify
{
    public static class IdentityConfig
    {
        public static void AddIdentityProviderConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITenantProvider, IdentityTenantProvider>();
            services.AddScoped<ITokenProvider, AccessTokenProvider>();

            services.AddIdentity<Usuario, Cargo>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddJwtAuthentication(configuration);
        }

        private static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var chaveAssinaturaJwt = configuration["JWT_GENERATION_KEY"];

            if (chaveAssinaturaJwt is null)
                throw new ArgumentException("Não foi possível obter a chave de assinatura de tokens.");

            var chaveEmBytes = Encoding.ASCII.GetBytes(chaveAssinaturaJwt);

            var audienciaValida = configuration["JWT_AUDIENCE_DOMAIN"];

            if (audienciaValida is null)
                throw new ArgumentException("Não foi possível obter o domínio da audiência dos tokens.");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(chaveEmBytes),
                    ValidAudience = audienciaValida,
                    ValidIssuer = "GestaoDeEstacionamento",
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true
                };
            });
        }
    }
}
