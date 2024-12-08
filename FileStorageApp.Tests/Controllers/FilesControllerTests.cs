using FileStorageApp.API.Controllers;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FileStorageApp.Tests.Controllers
{
    public class FilesControllerTests
    {
        private readonly Mock<IFileService> _mockFileService;
        private readonly FilesController _controller;

        public FilesControllerTests()
        {
            _mockFileService = new Mock<IFileService>();
            _controller = new FilesController(_mockFileService.Object);
        }

        [Fact]
        public async Task UploadFile_ReturnsOk_WhenFileIsUploaded()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "File content";
            var fileName = "test.txt";
            var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var fileDto = new CreateFileDto
            {
                File = fileMock.Object,
                FolderPath = "/testFolder"
            };

            var uploadedFile = new FileDto { FileName = fileName, Id = Guid.NewGuid() };
            _mockFileService
                .Setup(s => s.UploadFileAsync(It.IsAny<Stream>(), fileName, ms.Length, fileDto.FolderPath))
                .ReturnsAsync(uploadedFile);

            // Act
            var response = await _controller.UploadFile(fileDto);

            // Assert
            Assert.NotNull(response);
            var okResult = response.Result as OkObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
            Assert.Equal(uploadedFile, okResult?.Value);
        }

        [Fact]
        public async Task UploadFile_ReturnsBadRequest_WhenFileIsNull()
        {
            // Arrange
            var fileDto = new CreateFileDto { File = null };

            // Act
            var response = await _controller.UploadFile(fileDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public async Task UploadFileWithFileName_ReturnsOk_WhenFileIsUploaded()
        {
            // Arrange
            var fileDto = new CreateFileWithNameDto
            {
                FileName = "test.txt",
                FolderPath = "/testFolder"
            };

            var uploadedFile = new FileDto { FileName = fileDto.FileName, Id = Guid.NewGuid() };
            _mockFileService
                .Setup(s => s.UploadFileAsync(It.IsAny<Stream>(), fileDto.FileName, 0, fileDto.FolderPath))
                .ReturnsAsync(uploadedFile);

            // Act
            var response = await _controller.UploadFileWithFileName(fileDto);

            // Assert
            Assert.NotNull(response);
            var okResult = response.Result as OkObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
            Assert.Equal(uploadedFile, okResult?.Value);
        }

        [Fact]
        public async Task UploadFileWithFileName_ReturnsBadRequest_WhenFileNameIsEmpty()
        {
            // Arrange
            var fileDto = new CreateFileWithNameDto { FileName = "", FolderPath = "/testFolder" };

            // Act
            var response = await _controller.UploadFileWithFileName(fileDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public async Task GetFileMetadata_ReturnsOk_WithFileMetadata()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var fileMetadata = new FileDto { Id = fileId, FileName = "test.txt" };

            _mockFileService
                .Setup(s => s.GetFileMetadataAsync(fileId))
                .ReturnsAsync(fileMetadata);

            // Act
            var response = await _controller.GetFileMetadata(fileId);

            // Assert
            Assert.NotNull(response);
            var okResult = response.Result as OkObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
            Assert.Equal(fileMetadata, okResult?.Value);
        }

        [Fact]
        public async Task GetFileMetadata_ReturnsNotFound_WhenFileDoesNotExist()
        {
            // Arrange
            var fileId = Guid.NewGuid();

            _mockFileService
                .Setup(s => s.GetFileMetadataAsync(fileId))
                .ReturnsAsync((FileDto)null);

            // Act
            var response = await _controller.GetFileMetadata(fileId);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public async Task DownloadFile_ReturnsFileStream_WhenFileExists()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var fileName = "test.txt";
            var fileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("File content"));

            _mockFileService
                .Setup(s => s.DownloadFileAsync(fileId))
                .ReturnsAsync(fileStream);

            _mockFileService
                .Setup(s => s.GetFileMetadataAsync(fileId))
                .ReturnsAsync(new FileDto { FileName = fileName });

            // Act
            var result = await _controller.DownloadFile(fileId) as FileStreamResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("application/octet-stream", result.ContentType);
            Assert.Equal(fileName, result.FileDownloadName);
        }

        [Fact]
        public async Task DownloadFile_ReturnsNotFound_WhenFileDoesNotExist()
        {
            // Arrange
            var fileId = Guid.NewGuid();

            _mockFileService
                .Setup(s => s.DownloadFileAsync(fileId))
                .ReturnsAsync((Stream)null);

            // Act
            var result = await _controller.DownloadFile(fileId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteFile_ReturnsOk_WhenFileIsDeleted()
        {
            // Arrange
            var fileId = Guid.NewGuid();

            _mockFileService
                .Setup(s => s.DeleteFileAsync(fileId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteFile(fileId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True((bool)result.Value);
        }

        [Fact]
        public async Task DeleteFile_ReturnsNotFound_WhenFileDoesNotExist()
        {
            // Arrange
            var fileId = Guid.NewGuid();

            _mockFileService
                .Setup(s => s.DeleteFileAsync(fileId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteFile(fileId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}