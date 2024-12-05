using FileStorageApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using File = FileStorageApp.Core.Models.File;

namespace FileStorageApp.Infrastructure.Data
{
    public class FileStorageDbContext : DbContext
    {
        public FileStorageDbContext(DbContextOptions<FileStorageDbContext> options)
            : base(options)
        {
        }

        // DbSets for each entity
        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<FileVersion> FileVersions { get; set; }
        public DbSet<Share> Shares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // File configuration
            modelBuilder.Entity<File>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(f => f.Owner)
                    .WithMany(u => u.Files)
                    .HasForeignKey(f => f.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Folder)
                    .WithMany(f => f.Files)
                    .HasForeignKey(f => f.FolderId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Folder configuration
            modelBuilder.Entity<Folder>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(f => f.Owner)
                    .WithMany(u => u.Folders)
                    .HasForeignKey(f => f.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.ParentFolder)
                    .WithMany(f => f.Subfolders)
                    .HasForeignKey(f => f.ParentFolderId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Share configuration
            modelBuilder.Entity<Share>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(s => s.SharedBy)
                    .WithMany()
                    .HasForeignKey(s => s.SharedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.SharedWith)
                    .WithMany(u => u.Shares)
                    .HasForeignKey(s => s.SharedWithId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
