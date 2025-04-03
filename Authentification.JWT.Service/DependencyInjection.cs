using Authentification.JWT.DAL;
using Authentification.JWT.Service.Dto;
using Authentification.JWT.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authentification.JWT.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection("Jwt").Get<JwtConfig>();

        var key = Encoding.ASCII.GetBytes(jwtConfig?.SecretKey!);
        var tokenValidationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = jwtConfig?.Issuer!,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidAudience = jwtConfig?.Audience!,
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwt => {
            jwt.SaveToken = false;
            jwt.RequireHttpsMetadata = false;
            jwt.TokenValidationParameters = tokenValidationParams;
        });
        services.Configure<JwtConfig>(configuration.GetSection("Jwt"));

        services.AddDataAccessLayer(configuration);

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtService, JwtService>();
        return services;
    }
}