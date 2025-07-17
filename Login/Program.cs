using Login.API.Extensions.SwaggerConfigurations;
using Login.Application;
using Login.Repository;
using Login.API.Extensions;

/// <summary>
/// Classe principal do aplicativo Cliente API.
/// </summary>
public class Program
{
    /// <summary>
    /// Ponto de entrada principal do aplicativo.
    /// </summary>
    /// <param name="args">Argumentos de linha de comando.</param>
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuração de serviços
        builder.Services
            .AddSwaggerConfig(builder.Configuration)
            .AddControllers();

        builder.Services.AddCustomCors();

        builder.Services.AddRepository(builder.Configuration);
        builder.Services.AddIdentity();
        builder.Services.AddService(builder.Configuration);

        // Adiciona configuração JWT centralizada
        builder.Services.AddJwtAuthentication(builder.Configuration);

        var app = builder.Build();

        app.UsePathBase("/login-api");

        app.UseCustomCors();

        app.UseRouting();

        // Swagger
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/login/swagger/v1/swagger.json", "Login API V1");
            options.RoutePrefix = string.Empty;
        });

        // Adiciona autenticação e autorização JWT
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}