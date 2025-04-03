using Authentification.JWT.DAL.Data;
using Authentification.JWT.DAL.Exceptions;
using Authentification.JWT.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Authentification.JWT.DAL.Repositories;

public class UserRepository(ApplicationDbContext context,ILogger<UserRepository> logger) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<UserRepository> _logger = logger;
    public async Task<User> GetUserByUsernameAsync(string username) {
        var res =  await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if(res == null)
        {
            _logger.LogWarning("User with username {Username} not found", username);
            throw new UserNotFoundException($"User with username {username} not found");
        }
        return res;
    }

    public async Task<User> RegisterUserAsync(User user)
    {
        try
        {
            await _context.Users.AddAsync(user);
            var schanges = await _context.SaveChangesAsync();
            if (schanges <= 0)
            {
                _logger.LogError("Failed to register user {Username}", user.Username);
                throw new Exception("Failed to register user");
            }
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user {Username}", user.Username);
            throw;
        }
    }

    public bool VerifyPassword(int userId, string password)
    {
        User user = _context.Users.FirstOrDefault(u => u.Id == userId) ?? throw new KeyNotFoundException();
        var hashPassword = IUserRepository.HashPassword(password, user.Salt);
        return hashPassword == user.PasswordHash;
    }
}