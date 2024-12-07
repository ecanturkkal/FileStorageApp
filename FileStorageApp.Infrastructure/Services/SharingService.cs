using AutoMapper;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Models;
using FileStorageApp.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace FileStorageApp.Infrastructure.Services
{
    public class SharingService : ISharingService
    {
        private readonly FileStorageDbContext _context;
        private readonly ILogger<SharingService> _logger;
        private readonly IMapper _mapper;

        public SharingService(
            FileStorageDbContext context,
            IUserService userService,
            ILogger<SharingService> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public Task<ShareDto> CreateShareAsync(CreateShareRequestDto shareRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ShareDto>> GetSharesForResourceAsync(Guid resourceId)
        {
            throw new NotImplementedException();
        }

        public bool HasSharePermission(Guid resourceId, Guid userId)
        {
            return _context.Shares.Any(s => s.ResourceId == resourceId &&
                            s.SharedWithId == userId &&
                            s.Permission >= SharePermission.View);
        }
    }
}
