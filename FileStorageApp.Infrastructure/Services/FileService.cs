using AutoMapper;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Exceptions;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Models;
using FileStorageApp.Core.Utils;
using FileStorageApp.Core.Validation;
using Microsoft.Extensions.Logging;
using File = FileStorageApp.Core.Models.File;

namespace FileStorageApp.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IAzureBlobService _blobService;
        private readonly ILogger<FileService> _logger;
        private readonly IMapper _mapper;

        public FileService(
            IFileRepository fileRepository,
            IAzureBlobService blobService,
            ILogger<FileService> logger,
            IMapper mapper)
        {
            _fileRepository = fileRepository;
            _blobService = blobService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<FileDto> UploadFileAsync(Stream fileStream, string fileName, long fileSize, Guid? folderId)
        {
            try
            {            // Validate file
                FileValidator.ValidateFile(fileSize, fileName);

                // Generate unique blob path
                var blobPath = Utils.GenerateUniqueBlobPath(fileName);

                // Upload to blob storage
                var blobUrl = await _blobService.UploadBlobAsync(fileStream, blobPath);

                // Create file entity
                var file = new File
                {
                    FileName = fileName,
                    FileExtension = Path.GetExtension(fileName),
                    FileSize = fileSize,
                    StoragePath = blobPath,
                    OwnerId = new Guid("7bec920a-a90d-486b-d679-08dd15045002"),
                    FolderId = folderId,
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
                        CreatedById = new Guid("7bec920a-a90d-486b-d679-08dd15045002"),
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
                throw new FileNotFoundException($"File with ID {fileId} not found.");

            // Check user permissions
            //CheckFileAccessPermissions(file);

            // Download from blob storage
            return await _blobService.DownloadBlobAsync(file.StoragePath);
        }

        public async Task<bool> DeleteFileAsync(Guid fileId)
        {
            var file = await _fileRepository.GetByIdAsync(fileId);

            if (file == null)
                throw new FileNotFoundException($"File with ID {fileId} not found.");

            await _blobService.DeleteBlobAsync(file.StoragePath);
            var result = await _fileRepository.DeleteAsync(fileId);
            return result;
        }

        public Task<FileDto> GetFileMetadataAsync(Guid fileId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FileVersionDto>> GetFileVersionsAsync(Guid fileId)
        {
            throw new NotImplementedException();
        }

        //private void CheckFileAccessPermissions(File file)
        //{
        //    var currentUserId = _currentUserService.GetCurrentUserId();

        //    // Check if user is the owner or has been granted access
        //    if (file.OwnerId != currentUserId)
        //    {
        //        var hasPermission = _context.Shares
        //            .Any(s => s.ResourceId == file.Id &&
        //                       s.SharedWithId == currentUserId &&
        //                       s.Permission >= SharePermission.View);

        //        if (!hasPermission)
        //            throw new UnauthorizedAccessException("You do not have permission to access this file.");
        //    }
        //}
    }
}
