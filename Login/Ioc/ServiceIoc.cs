using Login.Interface.Service;
using Login.Service;

namespace Login.Ioc
{
    public static class ServiceIoc
    {
        public static void ConfigServiceIoc(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddHttpContextAccessor();

        }
    }
}