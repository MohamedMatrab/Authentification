using Authentification.JWT.DAL.Data;
using Authentification.JWT.DAL.Exceptions;
using Authentification.JWT.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentification.JWT.DAL.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<User> GetUserByUsernameAsync(string username) {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username) 
            ?? throw new UserNotFoundException("User With This UserName Not Found !");
    }

    public async Task<User> RegisterUserAsync(User user)
    {
        try
        {
            await _context.Users.AddAsync(user);
            var schanges = await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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