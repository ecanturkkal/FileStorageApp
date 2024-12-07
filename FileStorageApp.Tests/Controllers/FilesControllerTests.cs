using Xunit;
using Moq;
using FileStorageApp.API.Controllers;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FileStorageApp.Tests.Controllers
{
    public class FilesControllerTests
    {
        private readonly FilesController _controller;
        private readonly Mock<IFileService> _mockFileService;

        public FilesControllerTests()
        {
            _mockFileService = new Mock<IFileService>();
            _controller = new FilesController(_mockFileService.Object);
        }

        [Fact]
        public async Task UploadFile_ShouldReturnBadRequest_WhenNoFileIsUploaded()
        {
            // Arrange
            var fileDto = new CreateFileDto { File = null };

            // Act
            var result = await _controller.UploadFile(fileDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.Equal("No file uploaded.", badRequest?.Value);
        }

        [Fact]
        public async Task UploadFile_ShouldReturnOk_WhenFileIsUploaded()
        {
            // Arrange
            var fileDto = new CreateFileDto
            {
                File = new Mock<IFormFile>().Object,
                FolderPath = "some/path"
            };
            var uploadedFile = new FileDto { FileName = "file.txt", FileSize = 1024 };

            _mockFileService
                .Setup(s => s.UploadFileAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(uploadedFile);

            // Act
            var result = await _controller.UploadFile(fileDto);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(uploadedFile, okResult?.Value);
        }

        [Fact]
        public async Task UploadFileWithFileName_ShouldReturnBadRequest_WhenFileNameIsEmpty()
        {
            // Arrange
            var fileDto = new CreateFileWithNameDto { FileName = "" };

            // Act
            var result = await _controller.UploadFileWithFileName(fileDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.Equal("No file uploaded.", badRequest?.Value);
        }

        [Fact]
        public async Task UploadFileWithFileName_ShouldReturnOk_WhenFileIsUploaded()
        {
            // Arrange
            var fileDto = new CreateFileWithNameDto { FileName = "file.txt", FolderPath = "some/path" };
            var uploadedFile = new FileDto { FileName = "file.txt", FileSize = 1024 };

            _mockFileService
                .Setup(s => s.UploadFileAsync(It.IsAny<Stream>(), fileDto.FileName, 0, fileDto.FolderPath))
                .ReturnsAsync(uploadedFile);

            // Act
            var result = await _controller.UploadFileWithFileName(fileDto);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(uploadedFile, okResult?.Value);
        }

        [Fact]
        public async Task GetFileMetadata_ShouldReturnNotFound_WhenFileDoesNotExist()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            _mockFileService.Setup(s => s.GetFileMetadataAsync(fileId)).ReturnsAsync((FileDto)null);

            // Act
            var result = await _controller.GetFileMetadata(fileId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetFileMetadata_ShouldReturnOk_WhenFileExists()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var fileMetadata = new FileDto { FileName = "file.txt", FileSize = 1024 };

            _mockFileService.Setup(s => s.GetFileMetadataAsync(fileId)).ReturnsAsync(fileMetadata);

            // Act
            var result = await _controller.GetFileMetadata(fileId);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(fileMetadata, okResult.Value);
        }

        [Fact]
        public async Task DownloadFile_ShouldReturnNotFound_WhenFileDoesNotExist()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            _mockFileService.Setup(s => s.DownloadFileAsync(fileId)).ReturnsAsync((Stream)null);

            // Act
            var result = await _controller.DownloadFile(fileId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DownloadFile_ShouldReturnFile_WhenFileExists()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var fileStream = new MemoryStream();
            var fileMetadata = new FileDto { FileName = "file.txt" };

            _mockFileService.Setup(s => s.DownloadFileAsync(fileId)).ReturnsAsync(fileStream);
            _mockFileService.Setup(s => s.GetFileMetadataAsync(fileId)).ReturnsAsync(fileMetadata);

            // Act
            var result = await _controller.DownloadFile(fileId);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/octet-stream", fileResult.ContentType);
            Assert.Equal("file.txt", fileResult.FileDownloadName);
        }

        [Fact]
        public async Task DeleteFile_ShouldReturnNoContent_WhenFileIsDeleted()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            _mockFileService.Setup(s => s.DeleteFileAsync(fileId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteFile(fileId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteFile_ShouldReturnNotFound_WhenFileDoesNotExist()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            _mockFileService.Setup(s => s.DeleteFileAsync(fileId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteFile(fileId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }

}
