using FileStorageApp.Core.Exceptions;

namespace FileStorageApp.Core.Validation
{
    public class FileValidator
    {
        private const long MaxFileSize = 100 * 1024 * 1024; // 100 MB
        private static readonly string[] AllowedFileExtensions =
        {
            ".txt", ".pdf", ".docx", ".xlsx", ".pptx",
            ".jpg", ".png", ".gif", ".mp4", ".mp3"
        };

        public static void ValidateFile(long fileSize, string fileName)
        {
            // Check file size
            if (fileSize > MaxFileSize)
                throw new FileStorageException("File size exceeds maximum limit.");

            // Check file extension
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (!AllowedFileExtensions.Contains(extension))
                throw new FileStorageException("File type not allowed.");
        }
    }
}
