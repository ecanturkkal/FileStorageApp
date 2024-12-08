using FileStorageApp.API.Extensions;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;

    public AuthController(ITokenService tokenService, IUserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    /// <summary>
    /// Login to File Storage Api
    /// </summary>
    /// <param name="loginDto">Login params: username and email</param>
    /// <returns>Token</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
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
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}, Detail: {ex.InnerException}");
        }
    }

    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="userDto">Main user info</param>
    /// <returns>Created user</returns>
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
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}, Detail: {ex.InnerException}");
        }
    }

    /// <summary>
    /// List all users
    /// </summary>
    /// <returns>User list</returns>
    [HttpGet("getUsers")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        try
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}, Detail: {ex.InnerException}");
        }
    }

    private async Task<string> GetUserIdIfExists(LoginDto loginDto)
    {
        var user = await _userService.GetUserByUsernameAsync(loginDto.Username);

        if (user == null)
            return string.Empty;

        if (user.Username == loginDto.Username && user.Password == loginDto.Password)
            return user.Id.ToString();

        return string.Empty;
    }
}