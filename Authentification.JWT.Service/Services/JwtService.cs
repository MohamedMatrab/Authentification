using Authentification.JWT.DAL.Data;
using Authentification.JWT.DAL.Models;
using Authentification.JWT.Service.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentification.JWT.Service.Services;

public class JwtService(
    IOptionsMonitor<JwtConfig> jwtConfig,
    ApplicationDbContext dbContext,
    ILogger<JwtService> logger)
    : IJwtService
{
    private readonly JwtConfig _jwtConfig = jwtConfig.CurrentValue;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly ILogger<JwtService> _logger = logger;
    public string GenerateToken(User user)
    {
        try
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.SecretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var expirationDate = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryMinutes);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationDate,
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = signingCredentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            //var refreshToken = new JwtRefreshToken()
            //{
            //    JwtId = token.Id,
            //    IsUsed = false,
            //    IsRevoked = false,
            //    UserId = user.Id,
            //    CreatedAt = DateTime.UtcNow,
            //    ExpiredAt = DateTime.UtcNow.AddMonths(1),
            //    Token = RandomString + Guid.NewGuid()
            //};

            //_dbContext.JwtRefreshTokens.Add(refreshToken);
            //_dbContext.SaveChanges();

            _logger.LogInformation("Token generated for user {UserId} with expiration date {ExpirationDate}", user.Id, expirationDate);
            return jwtToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating token for user {UserId}", user.Id);
            throw;
        }
    }
    //private string RandomString
    //{
    //    get
    //    {
    //        var random = new Random();
    //        const string chars = "ABCDEFGHIJKLMNOPRSTUVYZWX0123456789";
    //        return new string(Enumerable.Repeat(chars, 35).Select(n => n[random.Next(n.Length)]).ToArray());
    //    }
    //}
}