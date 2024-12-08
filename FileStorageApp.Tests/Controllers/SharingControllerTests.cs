using FileStorageApp.API.Controllers;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FileStorageApp.Tests.Controllers
{
    public class SharingControllerTests
    {
        private readonly Mock<ISharingService> _mockSharingService;
        private readonly SharingController _controller;

        public SharingControllerTests()
        {
            _mockSharingService = new Mock<ISharingService>();
            _controller = new SharingController(_mockSharingService.Object);
        }

        [Fact]
        public async Task ShareResource_ReturnsOk_WhenSharingIsSuccessful()
        {
            // Arrange
            var shareRequest = new CreateShareRequestDto
            {
                ResourceId = Guid.NewGuid(),
                ResourceType = ResourceType.File,
                SharedWithId = Guid.NewGuid(),
                Permission = SharePermission.None,
                ExpiresAt = DateTime.UtcNow
            };

            var shareResult = new ShareDto
            {
                ResourceId = shareRequest.ResourceId,
                ResourceType = shareRequest.ResourceType,
                SharedWithName = "sharedUser",
                Permission = shareRequest.Permission,
                ExpiresAt = shareRequest.ExpiresAt
            };

            _mockSharingService
                .Setup(s => s.CreateShareAsync(It.IsAny<CreateShareRequestDto>()))
                .ReturnsAsync(shareResult);

            // Act
            var response = await _controller.ShareResource(shareRequest);

            // Assert
            Assert.NotNull(response);
            var okResult = response.Result as OkObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
        }

        [Fact]
        public async Task ShareResource_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("ResourceId", "ResourceId is required.");

            var shareRequest = new CreateShareRequestDto();

            // Act
            var response = await _controller.ShareResource(shareRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public async Task ShareResource_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var shareRequest = new CreateShareRequestDto
            {
                ResourceId = Guid.NewGuid(),
                ResourceType = ResourceType.File,
                SharedWithId = Guid.NewGuid(),
                Permission = SharePermission.None,
                ExpiresAt = DateTime.UtcNow
            };

            _mockSharingService
                .Setup(s => s.CreateShareAsync(It.IsAny<CreateShareRequestDto>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var response = await _controller.ShareResource(shareRequest);

            // Assert
            Assert.NotNull(response);
            var objResult = response.Result as ObjectResult;
            Assert.Equal(500, objResult?.StatusCode);
            Assert.Contains("Test exception", objResult?.Value.ToString());
        }

        [Fact]
        public async Task GetResourceShares_ReturnsOk_WhenSharesExist()
        {
            // Arrange
            var resourceId = Guid.NewGuid();
            var shares = new List<ShareDto>
            {
                 new ShareDto {
                    ResourceId = Guid.NewGuid(),
                    ResourceType = ResourceType.File,
                    SharedWithName = "sharedUser",
                    Permission = SharePermission.None,
                    ExpiresAt = DateTime.UtcNow
                 },
                 new ShareDto {
                    ResourceId = Guid.NewGuid(),
                    ResourceType = ResourceType.File,
                    SharedWithName = "sharedUser",
                    Permission = SharePermission.None,
                    ExpiresAt = DateTime.UtcNow
                 }
            };

            _mockSharingService
                .Setup(s => s.GetSharesForResourceAsync(It.IsAny<Guid>()))
                .ReturnsAsync(shares);

            // Act
            var response = await _controller.GetResourceShares(resourceId);

            // Assert
            Assert.NotNull(response);
            var okResult = response.Result as OkObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
        }

        [Fact]
        public async Task GetResourceShares_ReturnsNotFound_WhenNoSharesExist()
        {
            // Arrange
            var resourceId = Guid.NewGuid();

            _mockSharingService
                .Setup(s => s.GetSharesForResourceAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<ShareDto>());

            // Act
            var response = await _controller.GetResourceShares(resourceId);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public async Task GetResourceShares_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var resourceId = Guid.NewGuid();

            _mockSharingService
                .Setup(s => s.GetSharesForResourceAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var response = await _controller.GetResourceShares(resourceId);

            // Assert
            Assert.NotNull(response);
            var objResult = response.Result as ObjectResult;
            Assert.Equal(500, objResult?.StatusCode);
            Assert.Contains("Test exception", objResult?.Value.ToString());
        }
    }
}
