using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FileStorageApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoldersController : ControllerBase
    {
        private readonly IFolderService _folderService;

        public FoldersController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        /// <summary>
        /// Create a new folder
        /// </summary>
        /// <param name="folderDto">Folder creation details</param>
        /// <returns>Created folder details</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FolderDto>> CreateFolder([FromBody] CreateFolderDto folderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdFolder = await _folderService.CreateFolderAsync(folderDto);
            return CreatedAtAction(
                nameof(GetFolder),
                new { folderId = createdFolder.Id },
                createdFolder);
        }

        /// <summary>
        /// Get folder details and contents
        /// </summary>
        /// <param name="folderId">Unique identifier of the folder</param>
        /// <returns>Folder details with files and subfolders</returns>
        [HttpGet("{folderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FolderDetailsDto>> GetFolder(Guid folderId)
        {
            var folder = await _folderService.GetFolderDetailsAsync(folderId);

            if (folder == null)
                return NotFound();

            return Ok(folder);
        }

        /// <summary>
        /// Delete a folder and its contents
        /// </summary>
        /// <param name="folderId">Unique identifier of the folder</param>
        /// <returns>Deletion result</returns>
        [HttpDelete("{folderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFolder(Guid folderId)
        {
            var deleted = await _folderService.DeleteFolderAsync(folderId);

            return deleted ? NoContent() : NotFound();
        }
    }
}
