using Authentification.JWT.DAL.Models;
using System.Security.Cryptography;
using System.Text;

namespace Authentification.JWT.DAL.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> RegisterUserAsync(User user);
    bool VerifyPassword(int userId, string password);
    public static string HashPassword(string password, string salt)
    {
        using HMACSHA256 hmac = new(Encoding.UTF8.GetBytes(salt));
        byte[] bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        string hashedPassword = BitConverter.ToString(bytes).Replace("-", "").ToLower();
        return hashedPassword;
    }
}
