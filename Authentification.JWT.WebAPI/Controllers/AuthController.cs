using Authentification.JWT.DAL.Models;
using Authentification.JWT.Service.Dto;
using Authentification.JWT.Service.Services;
using Authentification.JWT.WebAPI.ActionModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentification.JWT.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IUserService userService, IJwtService jwtService,ILogger<AuthController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<AuthController> _logger = logger;
    private readonly IJwtService _jwtService = jwtService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid registration model");
            return BadRequest(ModelState);
        }
        try
        {
            _logger.LogInformation("Registering user with username: {Username}", model.Username);
            UserDto? existingUser;
            try
            {
                existingUser = await _userService.GetUserByUsernameAsync(model.Username);
            }
            catch (Exception)
            {
                existingUser = null;
            }
            if (existingUser != null)
            {
                _logger.LogWarning("Username {Username} is already taken", model.Username);
                return Conflict(new { message = "Username is already taken" });
            }

            var user = await _userService.RegisterUserAsync(model.Username, model.Email, model.Password);

            var userEntity = new User
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            var token = _jwtService.GenerateToken(userEntity);

            _logger.LogInformation("User {Username} registered successfully", model.Username);
            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                Token = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with username: {Username}", model.Username);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error registering user", error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid login model");
            return BadRequest(ModelState);
        }

        try
        {
            var user = await _userService.GetUserByUsernameAsync(model.Username);
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            if (!_userService.VerifyPassword(user, model.Password))
                return Unauthorized(new { message = "Invalid username or password" });

            var userEntity = new User
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            var token = _jwtService.GenerateToken(userEntity);

            _logger.LogInformation("User {Username} logged in successfully", model.Username);
            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                Token = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Username}", model.Username);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error during login", error = ex.Message });
        }
    }


    [Authorize]
    [HttpGet("protected")]
    public IActionResult ProtectedEndpoint()
    {
        return Ok("You have accessed a protected endpoint!");

    }
}