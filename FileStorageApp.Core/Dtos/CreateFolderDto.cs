namespace FileStorageApp.Core.Dtos
{
    public class CreateFolderDto
    {
        public required string Name { get; set; }
        public Guid? ParentFolderId { get; set; }
    }
}
