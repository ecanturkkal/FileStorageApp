using AutoMapper;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using Microsoft.Extensions.Logging;

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

        public Task<FolderDto> CreateFolderAsync(CreateFolderDto folderDto)
        {
            throw new NotImplementedException();
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
