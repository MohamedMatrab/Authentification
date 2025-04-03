namespace Authentification.JWT.WebAPI.ActionModels;

public class RegisterModel
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}