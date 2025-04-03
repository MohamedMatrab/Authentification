﻿namespace Authentification.JWT.DAL.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;
    public string Salt { get; set; } = default!;

}
