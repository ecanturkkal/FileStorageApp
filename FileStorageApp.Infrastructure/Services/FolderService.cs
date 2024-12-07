using AutoMapper;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Exceptions;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Models;
using Microsoft.Extensions.Logging;

namespace FileStorageApp.Infrastructure.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IUserService _userService;
        private readonly IAzureBlobService _blobService;
        private readonly ILogger<FolderService> _logger;
        private readonly IMapper _mapper;

        public FolderService(
            IFolderRepository folderRepository,
            IUserService userService,
            IAzureBlobService blobService,
            ILogger<FolderService> logger,
            IMapper mapper)
        {
            _folderRepository = folderRepository;
            _userService = userService;
            _blobService = blobService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<FolderDto> CreateFoldersAsync(string? folderPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(folderPath))
                    return new FolderDto();

                var existingFolder = await _folderRepository.GetFolderByStoragePathAsync(folderPath);
                if (existingFolder != null)
                    return _mapper.Map<FolderDto>(existingFolder);

                var folder = new FolderDto();
                var folders = folderPath.Split('/');
                var storagePath = "";

                foreach (var folderName in folders)
                {
                    if (!string.IsNullOrWhiteSpace(folderName))
                    {
                        storagePath = string.IsNullOrWhiteSpace(storagePath) ? folderName : $"{storagePath}/{folderName}";
                        existingFolder = await _folderRepository.GetFolderByNameAsync(folderName);
                        if (existingFolder != null)
                        {
                            folder = _mapper.Map<FolderDto>(existingFolder);
                            continue;
                        }
                        // Create folder entity
                        var entity = new Folder
                        {
                            Name = folderName,
                            ParentFolderId = folder.Id == Guid.Empty ? null : folder.Id,
                            OwnerId = _userService.GetCurrentUserId(),
                            StoragePath = storagePath,
                        };

                        // Save to database
                        var parent = await _folderRepository.AddAsync(entity);
                        folder = _mapper.Map<FolderDto>(parent);
                    }
                }

                // Return DTO
                return folder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file");
                throw new FileStorageException("Failed to upload file", ex);
            }
        }

        public async Task<bool> DeleteFolderAsync(Guid folderId)
        {
            var folder = await _folderRepository.GetByIdAsync(folderId);

            if (folder == null)
                throw new FileStorageException($"Folder with ID {folderId} not found.");

            await _blobService.DeleteBlobAsync(folder.StoragePath);

            var result = await _folderRepository.DeleteAsync(folderId);
            return result;
        }

        public async Task<FolderDetailsDto> GetFolderDetailsAsync(Guid folderId)
        {
            var folder = await _folderRepository.GetByIdAsync(folderId);
            return _mapper.Map<FolderDetailsDto>(folder);
        }
    }
}
