using FileStorageApp.Core.Exceptions;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using File = FileStorageApp.Core.Models.File;

namespace FileStorageApp.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly FileStorageDbContext _context;
        private readonly ILogger<FileRepository> _logger;

        public FileRepository(FileStorageDbContext context, ILogger<FileRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<File> AddAsync(File file)
        {
            try
            {
                await _context.Files.AddAsync(file);
                await _context.SaveChangesAsync();
                return file;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding file");
                throw new FileStorageException("Failed to add file", ex);
            }
        }

        public async Task UpdateAsync(File file)
        {
            try
            {
                _context.Files.Update(file);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating file");
                throw new FileStorageException("Failed to update file", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid fileId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var file = await _context.Files
                    .Include(f => f.Versions)
                    .Include(f => f.Shares)
                    .FirstOrDefaultAsync(f => f.Id == fileId);

                if (file == null)
                    return false;

                _context.Files.Remove(file);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error deleting file");
                throw new FileStorageException("Failed to delete file", ex);
            }
        }

        public async Task<File?> GetByIdAsync(Guid fileId)
        {
            return await _context.Files.Include(f => f.Owner)
                        .Include(f => f.Folder)
                        .Include(f => f.Versions)
                        .FirstOrDefaultAsync(f => f.Id == fileId);
        }

        public async Task<IEnumerable<File>> GetUserFilesAsync(Guid userId)
        {
            return await _context.Files.Where(f => f.OwnerId == userId)
                        .Include(f => f.Folder).ToListAsync();
        }
    }
}
