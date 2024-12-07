using FileStorageApp.Core.Exceptions;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Models;
using FileStorageApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FileStorageApp.Infrastructure.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly FileStorageDbContext _context;
        private readonly ILogger<FileRepository> _logger;

        public FolderRepository(FileStorageDbContext context, ILogger<FileRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Folder> AddAsync(Folder folder)
        {
            try
            {
                await _context.Folders.AddAsync(folder);
                await _context.SaveChangesAsync();
                return folder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding folder");
                throw new FileStorageException("Failed to add folder", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid folderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var folder = await _context.Folders
                    .Include(f => f.Files)
                    .Include(f => f.Subfolders)
                    .FirstOrDefaultAsync(f => f.Id == folderId);

                if (folder == null)
                    return false;

                _context.Folders.Remove(folder);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error deleting folder");
                throw new FileStorageException("Failed to delete folder", ex);
            }
        }
        
        public async Task<Folder?> GetByIdAsync(Guid folderId)
        {
            return await _context.Folders.Include(f => f.Owner)
                .Include(f => f.Subfolders)
                .Include(f => f.Files)
                .FirstOrDefaultAsync(f => f.Id == folderId);
        }

        public async Task<Folder?> GetFolderByStoragePathAsync(string storagePath)
        {
            return await _context.Folders.Where(f => f.StoragePath == storagePath).FirstOrDefaultAsync();
        }

        public async Task<Folder?> GetFolderByNameAsync(string name)
        {
            return await _context.Folders.Where(f => f.Name == name).FirstOrDefaultAsync();
        }
    }
}