using FileStorageApp.Core.Exceptions;

namespace FileStorageApp.Core.Utils
{
    public class FileServiceValidator
    {
        private const long MaxFileSizeMB = 100; // 100 MB
        private const long MaxFileSize = 100 * 1024 * 1024;

        private static readonly string[] AllowedFileExtensions =
        {
            ".txt", ".pdf", ".docx", ".xlsx", ".pptx", ".jpg", ".png", ".gif", ".mp4", ".mp3"
        };

        public static void ValidateUploadFileRequest(long fileSize, string fileName, string? folderPath)
        {
            //if (!string.IsNullOrWhiteSpace(folderId) && !string.IsNullOrWhiteSpace(folderPath))
            //    throw new FileStorageException("You cannot upload file with folderId and folderPath. " +
            //        "Either both must be empty or one of them must be existed.");

            //// Check folderId for Guid format
            //if (!string.IsNullOrWhiteSpace(folderId))
            //{
            //    try
            //    {
            //        new Guid(folderId);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new FileStorageException($"Invalid folder Id: {ex.Message}");
            //    }
            //}

            // Check folder path
            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                var folders = folderPath.Split('/');
                foreach (var folder in folders)
                {
                    if (folder.Contains('.'))
                        throw new FileStorageException($"Invalid folder name: {folder}");
                }
            }

            // Check file size
            if (fileSize > MaxFileSize)
                throw new FileStorageException($"File size exceeds maximum limit. Max limit : {MaxFileSizeMB} MB");

            // Check file extension
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(extension))
                throw new FileStorageException("Missing file extension.");

            if (!AllowedFileExtensions.Contains(extension))
                throw new FileStorageException($"File type not allowed. Please upload these files: {string.Join(',', AllowedFileExtensions)}");
        }
    }
}
