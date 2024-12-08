using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FileStorageApp.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockTokenService = new Mock<ITokenService>();
            _mockUserService = new Mock<IUserService>();
            _controller = new AuthController(_mockTokenService.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "testUser", Password = "testPassword" };
            var userId = "250f7dac-2b52-4d36-9f60-9e19119d44ac";
            var token = "mockToken";

            _mockUserService
                .Setup(s => s.GetUserByUsernameAsync(loginDto.Username))
                .ReturnsAsync(new UserDto { Id = Guid.Parse(userId), Username = loginDto.Username, Password = loginDto.Password });

            _mockTokenService
                .Setup(s => s.GenerateJwtToken(userId))
                .Returns(token);

            // Act
            var response = await _controller.Login(loginDto);

            // Assert
            Assert.NotNull(response);
            var okResult = response as OkObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "testUser", Password = "wrongPassword" };

            _mockUserService
                .Setup(s => s.GetUserByUsernameAsync(loginDto.Username))
                .ReturnsAsync(new UserDto { Username = loginDto.Username, Password = "correctPassword" });

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task CreateUser_ReturnsOk_WhenUserIsCreatedSuccessfully()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Username = "newUser", Password = "password", Email = "email" };
            var createdUser = new UserDto { Id = Guid.NewGuid(), Username = createUserDto.Username };

            _mockUserService
                .Setup(s => s.CreateUserAsync(createUserDto))
                .ReturnsAsync(createdUser);

            // Act
            var response = await _controller.CreateUser(createUserDto);

            // Assert
            Assert.NotNull(response);
            var okResult = response.Result as OkObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
            Assert.Equal(createdUser, okResult?.Value);
        }

        [Fact]
        public async Task CreateUser_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Username", "Required");

            // Act
            var resppnse = await _controller.CreateUser(new CreateUserDto { Username = "", Password = "", Email = "" });

            // Assert
            Assert.IsType<BadRequestObjectResult>(resppnse.Result);
        }

        [Fact]
        public async Task GetUsers_ReturnsOk_WithListOfUsers()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto { Id = Guid.NewGuid(), Username = "User1" },
                new UserDto { Id = Guid.NewGuid(), Username = "User2" }
            };

            _mockUserService
                .Setup(s => s.GetUsersAsync())
                .ReturnsAsync(users);

            // Act
            var response = await _controller.GetUsers();

            // Assert
            Assert.NotNull(response);
            var okResult = response.Result as OkObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
            Assert.Equal(users, okResult?.Value);
        }

        [Fact]
        public async Task Login_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "testUser", Password = "testPassword" };

            _mockUserService
                .Setup(s => s.GetUserByUsernameAsync(loginDto.Username))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Test exception", result.Value.ToString());
        }
    }
}

