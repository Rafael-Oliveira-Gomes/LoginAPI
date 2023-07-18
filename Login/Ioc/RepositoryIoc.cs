using Login.Interface.Repository;
using Login.Repository;

namespace Login.Ioc
{
    public static class RepositoryIoc
    {
        public static void ConfigRepositoryIoc(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}