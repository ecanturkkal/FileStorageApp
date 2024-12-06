using AutoMapper;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Exceptions;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Models;
using FileStorageApp.Core.Utils;
using FileStorageApp.Core.Validation;
using FileStorageApp.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.IO;

namespace FileStorageApp.Infrastructure.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IAzureBlobService _blobService;
        private readonly ILogger<FolderService> _logger;
        private readonly IMapper _mapper;

        public FolderService(
            IFolderRepository folderRepository,
            IAzureBlobService blobService,
            ILogger<FolderService> logger,
            IMapper mapper)
        {
            _folderRepository = folderRepository;
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

                var existingFolder = await _folderRepository.GetFolderByFullDirectoryAsync(folderPath);
                if (existingFolder != null)
                    return _mapper.Map<FolderDto>(existingFolder);

                var folder = new FolderDto();
                var folders = folderPath.Split('/');
                var fullDirectory = "";

                foreach (var folderName in folders)
                {
                    if (!string.IsNullOrWhiteSpace(folderName))
                    {
                        fullDirectory = string.IsNullOrWhiteSpace(fullDirectory) ? folderName : $"{fullDirectory}/{folderName}";
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
                            OwnerId = new Guid("79d594f1-2c32-40b5-9718-08dd15fa4367"),
                            CreatedAt = DateTime.UtcNow,
                            FullDirectory = fullDirectory
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

        public Task<bool> DeleteFolderAsync(Guid folderId)
        {
            throw new NotImplementedException();
        }

        public Task<FolderDetailsDto> GetFolderDetailsAsync(Guid folderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FolderDto>> GetUserFoldersAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
