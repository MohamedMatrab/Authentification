using Authentification.JWT.DAL.Models;
using Authentification.JWT.DAL.Repositories;
using Authentification.JWT.Service.Dto;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SharedDependencies.Dtos;

namespace Authentification.JWT.Service.Services;
public class UserService(IUserRepository userRepository,IMapper mapper,ILogger<UserService> logger) : IUserService
{

    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UserService> _logger = logger;
    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        _logger.LogWarning("User with username {Username} not found", username);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> RegisterUserAsync(string username, string email, string password)
    {
        string salt = $"{username}@@@{email}";
        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = IUserRepository.HashPassword(password,salt),
            Salt = salt
        };

        await _userRepository.RegisterUserAsync(user);

        _logger.LogInformation("User with username {Username} registered successfully", username);
        return _mapper.Map<UserDto>(user);
    }

    public bool VerifyPassword(UserDto user, string password)
    {
        return _userRepository.VerifyPassword(user.Id, password);
    }
}