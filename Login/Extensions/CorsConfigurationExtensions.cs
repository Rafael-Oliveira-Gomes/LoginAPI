namespace Login.API.Extensions;

/// <summary>
/// Provides extension methods for configuring CORS in the application.
/// </summary>
public static class CorsConfigurationExtensions
{
    private const string CorsPolicyName = "AllowLocalhost5000";

    /// <summary>
    /// Adds a custom CORS policy to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the CORS policy to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
        return services;
    }

    /// <summary>
    /// Configures the application to use the custom CORS policy.
    /// </summary>
    /// <param name="app">The application builder to configure.</param>
    /// <returns>The updated application builder.</returns>
    public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
    {
        app.UseCors(CorsPolicyName);
        return app;
    }
}
