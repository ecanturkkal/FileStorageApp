using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FileStorageApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SharingController : ControllerBase
    {
        private readonly ISharingService _sharingService;

        public SharingController(ISharingService sharingService)
        {
            _sharingService = sharingService;
        }

        /// <summary>
        /// Share a file or folder with specific users
        /// </summary>
        /// <param name="shareRequest">Sharing details</param>
        /// <returns>Sharing result</returns>
        [HttpPost("share")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ShareDto>> ShareResource([FromBody] CreateShareRequestDto shareRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var shareResult = await _sharingService.CreateShareAsync(shareRequest);
            return Ok(shareResult);
        }

        /// <summary>
        /// Get sharing details for a resource
        /// </summary>
        /// <param name="resourceId">Unique identifier of the shared resource</param>
        /// <returns>Sharing details</returns>
        [HttpGet("{resourceId}/sharing")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ShareDto>>> GetResourceShares(Guid resourceId)
        {
            var shares = await _sharingService.GetSharesForResourceAsync(resourceId);

            if (shares == null || !shares.Any())
                return NotFound();

            return Ok(shares);
        }
    }
}
