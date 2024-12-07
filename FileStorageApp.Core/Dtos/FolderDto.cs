namespace FileStorageApp.Core.Dtos
{
    public class FolderDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentFolderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Owner { get; set; }
        public int FileCount { get; set; }
        public int SubfolderCount { get; set; }
    }
}
