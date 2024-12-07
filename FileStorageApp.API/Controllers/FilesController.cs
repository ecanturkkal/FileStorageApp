using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileStorageApp.API.Controllers
{
    [Authorize]
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
            try
            {
                if (fileDto.File == null || fileDto.File.Length == 0)
                    return BadRequest("No file uploaded.");

                var uploadResult = await _fileService.UploadFileAsync(
                    fileDto.File.OpenReadStream(),
                    fileDto.File.FileName,
                    fileDto.File.Length,
                    fileDto.FolderPath);

                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                // Detailed logging of the full exception
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Upload a file to a specific folder
        /// </summary>
        /// <param name="fileDto">The file to upload</param>
        /// <returns>Details of the uploaded file</returns>
        [HttpPost("upload-2")]
        public async Task<ActionResult<FileDto>> UploadFileWithFileName([FromBody] CreateFileWithNameDto fileDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileDto.FileName))
                    return BadRequest("No file uploaded.");

                var uploadResult = await _fileService.UploadFileAsync(
                new MemoryStream(),
                fileDto.FileName,
                0,
                fileDto.FolderPath);

                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                // Detailed logging of the full exception
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}, Reason: {ex.InnerException}");
            }
        }

        /// <summary>
        /// Get file metadata by file ID
        /// </summary>
        /// <param name="fileId">Unique identifier of the file</param>
        /// <returns>File metadata</returns>
        [HttpGet("{fileId}")]
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
        public async Task<ActionResult<IEnumerable<FileVersionDto>>> GetFileVersions(Guid fileId)
        {
            var versions = await _fileService.GetFileVersionsAsync(fileId);

            if (versions == null || !versions.Any())
                return NotFound();

            return Ok(versions);
        }
    }
}
