﻿using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileStorageApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Upload a file to a specific folder
        /// </summary>
        /// <param name="fileDto">The file to upload</param>
        /// <returns>Details of the uploaded file</returns>
        [HttpPost("upload")]
        public async Task<ActionResult<FileDto>> UploadFile([FromForm] CreateFileDto fileDto)
        {
            if (fileDto.File == null || fileDto.File.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                var uploadResult = await _fileService.UploadFileAsync(
                    fileDto.File.OpenReadStream(),
                    fileDto.File.FileName,
                    fileDto.File.Length,
                    fileDto.FolderId);

                return Ok();
            }
            catch (Exception ex)
            {
                // Detailed logging of the full exception
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Get file metadata by file ID
        /// </summary>
        /// <param name="fileId">Unique identifier of the file</param>
        /// <returns>File metadata</returns>
        [HttpGet("{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FileDto>> GetFileMetadata(Guid fileId)
        {
            var file = await _fileService.GetFileMetadataAsync(fileId);

            if (file == null)
                return NotFound();

            return Ok(file);
        }

        /// <summary>
        /// Download a file by its ID
        /// </summary>
        /// <param name="fileId">Unique identifier of the file</param>
        /// <returns>File download</returns>
        [HttpGet("download/{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadFile(Guid fileId)
        {
            var fileStream = await _fileService.DownloadFileAsync(fileId);

            if (fileStream == null)
                return NotFound();

            var file = await _fileService.GetFileMetadataAsync(fileId);
            return File(fileStream, "application/octet-stream", file.FileName);
        }

        /// <summary>
        /// Delete a file by its ID
        /// </summary>
        /// <param name="fileId">Unique identifier of the file</param>
        /// <returns>Deletion result</returns>
        [HttpDelete("{fileId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            var deleted = await _fileService.DeleteFileAsync(fileId);
            return deleted ? NoContent() : NotFound();
        }

        /// <summary>
        /// Get file versions for a specific file
        /// </summary>
        /// <param name="fileId">Unique identifier of the file</param>
        /// <returns>List of file versions</returns>
        [HttpGet("{fileId}/versions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FileVersionDto>>> GetFileVersions(Guid fileId)
        {
            var versions = await _fileService.GetFileVersionsAsync(fileId);

            if (versions == null || !versions.Any())
                return NotFound();

            return Ok(versions);
        }
    }
}
