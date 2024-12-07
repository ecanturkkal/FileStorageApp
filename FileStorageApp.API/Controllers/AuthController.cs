using FileStorageApp.API.Extensions;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly IUserService _userService;

    public AuthController(TokenService tokenService, IUserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        // Validate user credentials 
        var userId = await GetUserIdIfExists(loginDto);
        if (!string.IsNullOrWhiteSpace(userId))
        {
            var token = _tokenService.GenerateJwtToken(userId);
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }

    [HttpPost("createUser")]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUser = await _userService.CreateUserAsync(userDto);
            return Ok(createdUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}, Reason: {ex.InnerException}");
        }
    }

    [HttpGet("getUsers")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return Ok(users);
    }

    private async Task<string> GetUserIdIfExists(LoginDto loginDto)
    {
        var user = await _userService.GetUserAsync(loginDto.Username);

        if (user == null)
            return string.Empty;

        if (user.Username == loginDto.Username && user.Email == loginDto.Email)
            return user.Id.ToString();

        return string.Empty;
    }
}