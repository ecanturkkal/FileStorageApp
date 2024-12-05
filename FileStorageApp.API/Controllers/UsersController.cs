using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FileStorageApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userDto">User creation details</param>
        /// <returns>Created user details</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUser = await _userService.CreateUserAsync(userDto);
            return CreatedAtAction(
                nameof(GetUser),
                new { username = createdUser.Username },
                createdUser);
        }

        /// <summary>
        /// Get user details
        /// </summary>
        /// <param name="username">Unique identifier of the user</param>
        /// <returns>User details</returns>
        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUser(string username)
        {
            var user = await _userService.GetUserAsync(username);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
