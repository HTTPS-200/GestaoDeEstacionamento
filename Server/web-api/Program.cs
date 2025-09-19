using GestaoDeEstacionamento.Core.Aplicacao;
using GestaoDeEstacionamento.WebApi.AutoMapper;
using GestaoDeEstacionamento.WebApi.Identify;
using GestaoDeEstacionamento.WebApi.Orm;
using GestaoDeEstacionamento.WebApi.Swagger;
using System.Text.Json.Serialization;
using GestaoDeEstacionamento.Infraestrutura.Orm;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;

namespace eAgenda.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services
            .AddCamadaAplicacao(builder.Logging, builder.Configuration)
            .AddCamadaInfraestruturaOrm(builder.Configuration);

        builder.Services.AddAutoMapperProfiles(builder.Configuration);

        builder.Services.AddIdentityProviderConfig(builder.Configuration);


        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            );

        // Swagger/OpenAPI https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddSwaggerConfig();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            var contexto = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            contexto.Database.EnsureDeleted();

            contexto.Database.EnsureCreated();

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
