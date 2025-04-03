using Authentification.JWT.DAL.Models;
using Authentification.JWT.DAL.Repositories;
using Authentification.JWT.Service.Dto;
using AutoMapper;

namespace Authentification.JWT.Service.Services;
public class UserService(IUserRepository userRepository,IMapper mapper) : IUserService
{

    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        return  _mapper.Map<UserDto>(user);
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
        return _mapper.Map<UserDto>(user);
    }

    public bool VerifyPassword(UserDto user, string password)
    {
        return _userRepository.VerifyPassword(user.Id, password);
    }
}