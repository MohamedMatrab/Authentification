namespace Authentification.JWT.WebAPI.ActionModels;

public record LoginModel
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}