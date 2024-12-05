using Microsoft.AspNetCore.Http;

namespace FileStorageApp.Core.Dtos
{
    public class CreateFileDto
    {
        public required IFormFile File { get; set; }
        public Guid FolderId { get; set; } = Guid.Empty;
    }
}
