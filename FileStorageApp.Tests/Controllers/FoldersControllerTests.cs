using FileStorageApp.API.Controllers;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FileStorageApp.Tests.Controllers
{
    public class FoldersControllerTests
    {
        private readonly Mock<IFolderService> _mockFolderService;
        private readonly FoldersController _controller;

        public FoldersControllerTests()
        {
            _mockFolderService = new Mock<IFolderService>();
            _controller = new FoldersController(_mockFolderService.Object);
        }

        [Fact]
        public async Task GetFolder_ReturnsOk_WithFolderDetails()
        {
            // Arrange
            var folderId = Guid.NewGuid();
            var folderDetails = new FolderDetailsDto
            {
                Id = folderId,
                Name = "Test Folder",
                Files = new List<FileDto>(),
                Subfolders = new List<FolderDetailsDto>()
            };

            _mockFolderService
                .Setup(s => s.GetFolderDetailsAsync(folderId))
                .ReturnsAsync(folderDetails);

            // Act
            var response = await _controller.GetFolder(folderId);

            // Assert
            Assert.IsType<OkObjectResult>(response.Result);
            var okResult = response.Result as OkObjectResult;
            Assert.Equal(folderDetails, okResult?.Value);
        }

        [Fact]
        public async Task GetFolder_ReturnsNotFound_WhenFolderDoesNotExist()
        {
            // Arrange
            var folderId = Guid.NewGuid();

            _mockFolderService
                .Setup(s => s.GetFolderDetailsAsync(folderId))
                .ReturnsAsync((FolderDetailsDto)null);

            // Act
            var response = await _controller.GetFolder(folderId);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public async Task GetFolder_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var folderId = Guid.NewGuid();

            _mockFolderService
                .Setup(s => s.GetFolderDetailsAsync(folderId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var response = await _controller.GetFolder(folderId);

            // Assert
            Assert.NotNull(response);
            var objResult = response.Result as ObjectResult;
            Assert.Equal(500, objResult?.StatusCode);
            Assert.Contains("Test exception", objResult?.Value.ToString());
        }

        [Fact]
        public async Task DeleteFolder_ReturnsOk_WhenFolderIsDeleted()
        {
            // Arrange
            var folderId = Guid.NewGuid();

            _mockFolderService
                .Setup(s => s.DeleteFolderAsync(folderId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteFolder(folderId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True((bool)result.Value);
        }

        [Fact]
        public async Task DeleteFolder_ReturnsNotFound_WhenFolderDoesNotExist()
        {
            // Arrange
            var folderId = Guid.NewGuid();

            _mockFolderService
                .Setup(s => s.DeleteFolderAsync(folderId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteFolder(folderId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteFolder_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var folderId = Guid.NewGuid();

            _mockFolderService
                .Setup(s => s.DeleteFolderAsync(folderId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.DeleteFolder(folderId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Test exception", result.Value.ToString());
        }
    }
}

