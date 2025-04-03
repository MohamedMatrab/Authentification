using Authentification.JWT.DAL.Data;
using Authentification.JWT.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentification.JWT.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });
        services.AddTransient<IUserRepository,UserRepository>();
        return services;
    }
}