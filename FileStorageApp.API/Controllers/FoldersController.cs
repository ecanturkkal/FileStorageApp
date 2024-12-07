using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FileStorageApp.API.Controllers
{
    [Authorize]
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
        /// Get folder details and contents
        /// </summary>
        /// <param name="folderId">Unique identifier of the folder</param>
        /// <returns>Folder details with files and subfolders</returns>
        [HttpGet("{folderId}")]
        public async Task<ActionResult<FolderDetailsDto>> GetFolder(Guid folderId)
        {
            try
            {
                var folder = await _folderService.GetFolderDetailsAsync(folderId);

                if (folder == null)
                    return NotFound();

                return Ok(folder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}, Detail: {ex.InnerException}");
            }
        }

        /// <summary>
        /// Delete a folder and its contents
        /// </summary>
        /// <param name="folderId">Unique identifier of the folder</param>
        /// <returns>Deletion result</returns>
        [HttpDelete("{folderId}")]
        public async Task<IActionResult> DeleteFolder(Guid folderId)
        {
            try
            {
                var deleted = await _folderService.DeleteFolderAsync(folderId);
                return deleted ? Ok(deleted) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}, Detail: {ex.InnerException}");
            }
        }
    }
}
