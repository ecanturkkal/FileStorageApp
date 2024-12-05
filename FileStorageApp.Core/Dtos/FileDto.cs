﻿using FileStorageApp.Core.Models;

namespace FileStorageApp.Core.Dtos
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public Guid? FolderId { get; set; }
        public SharePermission UserPermission { get; set; }
    }
}
