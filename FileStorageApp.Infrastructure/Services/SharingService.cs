using AutoMapper;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Exceptions;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Models;
using FileStorageApp.Core.Utils;
using FileStorageApp.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace FileStorageApp.Infrastructure.Services
{
    public class SharingService : ISharingService
    {
        private readonly FileStorageDbContext _context;
        private readonly IShareRepository _shareRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUserService _userService;
        private readonly ILogger<SharingService> _logger;
        private readonly IMapper _mapper;

        public SharingService(
            FileStorageDbContext context,
            IShareRepository shareRepository,
            IFolderRepository folderRepository,
            IFileRepository fileRepository,
            IUserService userService,
            ILogger<SharingService> logger,
            IMapper mapper)
        {
            _context = context;
            _shareRepository = shareRepository;
            _folderRepository = folderRepository;
            _fileRepository = fileRepository;
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ShareDto> CreateShareAsync(CreateShareRequestDto shareRequest)
        {
            try
            {
                // Validate request
                SharingServiceValidator.ValidateCreateShareRequest(shareRequest);

                var resourceOwnerId = Guid.Empty;

                if (shareRequest.ResourceType == ResourceType.Folder)
                {
                    var folder = await _folderRepository.GetByIdAsync(shareRequest.ResourceId);
                    if (folder != null)
                        resourceOwnerId = folder.OwnerId;
                }

                if (shareRequest.ResourceType == ResourceType.File)
                {
                    var file = await _fileRepository.GetByIdAsync(shareRequest.ResourceId);
                    if (file != null)
                        resourceOwnerId = file.OwnerId;
                }

                var hasPermission = await HasUserResourceAccessPermissions(shareRequest.ResourceId, resourceOwnerId);
                if (!hasPermission)
                    throw new FileStorageException("You do not have permission to access this resource.");

                // Create share entity
                var share = new Share
                {
                    ResourceId = shareRequest.ResourceId,
                    ResourceType = shareRequest.ResourceType,
                    SharedById = _userService.GetCurrentUserId(),
                    SharedWithId = shareRequest.SharedWithId,
                    Permission = shareRequest.Permission,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = shareRequest.ExpiresAt,
                };

                await _shareRepository.AddAsync(share);
                return _mapper.Map<ShareDto>(share);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating share");
                throw new FileStorageException("Failed to create share", ex);
            }
        }

        public async Task<IEnumerable<ShareDto>> GetSharesForResourceAsync(Guid resourceId)
        {
            var shares = await _shareRepository.GetSharesByResourceIsAsync(resourceId);
            return _mapper.Map<IEnumerable<ShareDto>>(shares);
        }

        public async Task<bool> HasUserResourceAccessPermissions(Guid resourceId, Guid ownerId)
        {
            var currentUserId = _userService.GetCurrentUserId();
            if (ownerId == currentUserId)
                return true;

            var shares = await _shareRepository.GetSharesByResourceIsAsync(resourceId);

            return shares.Any(s => s.SharedWithId == currentUserId &&
                                   s.ExpiresAt >= DateTime.UtcNow &&
                                   s.Permission >= SharePermission.View);
        }
    }
}
