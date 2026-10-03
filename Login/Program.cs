
using Login.API.Extensions.SwaggerConfigurations;
using Login.Application;
using Login.Repository;
using Login.API.Extensions;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddSwaggerConfig(builder.Configuration)
            .AddControllers();

        builder.Services.AddCustomCors();

        builder.Services.AddRepository(builder.Configuration);
        builder.Services.AddIdentity();
        builder.Services.AddService(builder.Configuration);

        builder.Services.AddJwtAuthentication(builder.Configuration);

        var app = builder.Build();

        app.UsePathBase("/login-api");

        app.UseCustomCors();

        app.UseRouting();

        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint(
                "/login-api/swagger/v1/swagger.json",
                "Login API V1");

            options.RoutePrefix = string.Empty;
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        await app.RunAsync();
    }
}