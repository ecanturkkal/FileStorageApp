namespace FileStorageApp.Core.Dtos
{
    public class CreateFileWithNameDto
    {
        public required string FileName { get; set; }
        public string? FolderPath { get; set; }
    }
}
