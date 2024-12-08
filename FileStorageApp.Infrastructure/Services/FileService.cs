using AutoMapper;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Exceptions;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Models;
using FileStorageApp.Core.Utils;
using Microsoft.Extensions.Logging;
using File = FileStorageApp.Core.Models.File;

namespace FileStorageApp.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFolderService _folderService;
        private readonly IUserService _userService;
        private readonly IAzureBlobService _blobService;
        private readonly ISharingService _sharingService;
        private readonly ILogger<FileService> _logger;
        private readonly IMapper _mapper;

        public FileService(
            IFileRepository fileRepository,
            IFolderService folderService,
            IUserService userService,
            IAzureBlobService blobService,
            ISharingService sharingService,
            ILogger<FileService> logger,
            IMapper mapper)
        {
            _fileRepository = fileRepository;
            _folderService = folderService;
            _userService = userService;
            _blobService = blobService;
            _sharingService = sharingService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<FileDto> UploadFileAsync(Stream fileStream, string fileName, long fileSize, string? folderPath)
        {
            try
            {
                // Validate file
                FileServiceValidator.ValidateUploadFileRequest(fileSize, fileName, folderPath);
                var blobPath = fileName;
                var folderId = Guid.Empty;

                if (!string.IsNullOrWhiteSpace(folderPath))
                {
                    blobPath = folderPath.EndsWith("/") ? $"{folderPath}{fileName}" : $"{folderPath}/{fileName}";
                    var folder = await _folderService.CreateFoldersAsync(folderPath);
                    folderId = folder.Id;
                }

                // Upload to blob storage
                var blobUrl = await _blobService.UploadBlobAsync(fileStream, blobPath);

                // Create file entity
                var file = new File
                {
                    FileName = fileName,
                    FileExtension = Path.GetExtension(fileName),
                    FileSize = fileSize,
                    StoragePath = blobPath,
                    OwnerId = _userService.GetCurrentUserId(),
                    FolderId = folderId == Guid.Empty ? null : folderId,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow
                };

                // Add version
                file.Versions = new List<FileVersion>
                {
                    new FileVersion
                    {
                        StoragePath = blobPath,
                        CreatedAt = DateTime.UtcNow,
                        CreatedById = _userService.GetCurrentUserId(),
                    }
                };

                // Save to database
                await _fileRepository.AddAsync(file);

                // Return DTO
                return _mapper.Map<FileDto>(file);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file");
                throw new FileStorageException("Failed to upload file", ex);
            }
        }

        public async Task<Stream> DownloadFileAsync(Guid fileId)
        {
            var file = await _fileRepository.GetByIdAsync(fileId);

            if (file == null)
                throw new FileStorageException($"File with ID {fileId} not found.");

            // Check user permissions
            var hasPermission = await _sharingService.HasUserResourceAccessPermissions(fileId, file.OwnerId);
            if (!hasPermission)
                throw new FileStorageException("You do not have permission to access this file.");

            // Download from blob storage
            return await _blobService.DownloadBlobAsync(file.StoragePath);
        }

        public async Task<bool> DeleteFileAsync(Guid fileId)
        {
            var file = await _fileRepository.GetByIdAsync(fileId);

            if (file == null)
                throw new FileStorageException($"File with ID {fileId} not found.");

            // Check user permissions
            var hasPermission = await _sharingService.HasUserResourceAccessPermissions(fileId, file.OwnerId);
            if (!hasPermission)
                throw new FileStorageException("You do not have permission to access this file.");

            await _blobService.DeleteBlobAsync(file.StoragePath);
            var result = await _fileRepository.DeleteAsync(fileId);
            return result;
        }

        public async Task<FileDto> GetFileMetadataAsync(Guid fileId)
        {
            var file = await _fileRepository.GetByIdAsync(fileId);

            if (file == null)
                throw new FileStorageException($"File with ID {fileId} not found.");

            return _mapper.Map<FileDto>(file);
        }
    }
}
