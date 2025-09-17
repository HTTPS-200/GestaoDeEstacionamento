using GestaoDeEstacionamento.Core.Aplicacao;
using GestaoDeEstacionamento.WebApi.AutoMapper;
using GestaoDeEstacionamento.WebApi.Identify;
using GestaoDeEstacionamento.WebApi.Orm;
using GestaoDeEstacionamento.WebApi.Swagger;
using System.Text.Json.Serialization;
using GestaoDeEstacionamento.Infraestrutura.Orm;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;

namespace eAgenda.WebApi;

public class Program
{
    public static void Main(string[] args)
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
            app.ApplyMigrations();

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
