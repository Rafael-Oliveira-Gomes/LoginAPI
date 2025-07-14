using Login.Domain.Interfaces.Repositories;
using Login.Repository.Context;
using Login.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Login.Repository;

public static class AddRepositorySetup
{
    public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PostgresContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
        });
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}