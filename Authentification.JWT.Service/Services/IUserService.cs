using Authentification.JWT.Service.Dto;
using SharedDependencies.Dtos;

namespace Authentification.JWT.Service.Services;

public interface IUserService
{
    Task<UserDto> GetUserByUsernameAsync(string username);
    Task<UserDto> RegisterUserAsync(string username, string email, string password);
    bool VerifyPassword(UserDto user, string password);
}