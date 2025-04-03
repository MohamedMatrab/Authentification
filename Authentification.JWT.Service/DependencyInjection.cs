using Authentification.JWT.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Authentification.JWT.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtService, JwtService>();
        return services;
    }
}