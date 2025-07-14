using Login.Domain.Entities;
using Login.Repository.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Login.Application;

public static class AddIdentitySetup
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<PostgresContext>()
            .AddDefaultTokenProviders();
        return services;
    }
}
