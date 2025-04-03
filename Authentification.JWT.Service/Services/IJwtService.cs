using Authentification.JWT.DAL.Models;

namespace Authentification.JWT.Service.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}