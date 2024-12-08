using FileStorageApp.Core.Exceptions;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Models;
using FileStorageApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FileStorageApp.Infrastructure.Repositories
{
    public class ShareRepository : IShareRepository
    {
        private readonly FileStorageDbContext _context;
        private readonly ILogger<ShareRepository> _logger;

        public ShareRepository(FileStorageDbContext context, ILogger<ShareRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Share> AddAsync(Share share)
        {
            try
            {
                await _context.Shares.AddAsync(share);
                await _context.SaveChangesAsync();
                return share;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding share");
                throw new FileStorageException("Failed to add share", ex);
            }
        }

        public async Task<IEnumerable<Share>> GetSharesByResourceIsAsync(Guid resourceId)
        {
            return await _context.Shares.Where(f => f.ResourceId == resourceId)
                        .Include(f => f.SharedBy)
                        .Include(f => f.SharedWith).ToListAsync();
        }
    }
}