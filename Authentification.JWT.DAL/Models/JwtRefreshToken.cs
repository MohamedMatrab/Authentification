namespace Authentification.JWT.DAL.Models;

public class JwtRefreshToken
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? Token { get; set; }
    public string? JwtId { get; set; }
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiredAt { get; set; }

    public virtual User User { get; set; } = null!;
}