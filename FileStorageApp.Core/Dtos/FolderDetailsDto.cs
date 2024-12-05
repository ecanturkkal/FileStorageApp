namespace FileStorageApp.Core.Dtos
{
    public class FolderDetailsDto : FolderDto
    {
        public IEnumerable<FileDto> Files { get; set; }
        public IEnumerable<FolderDto> Subfolders { get; set; }
    }
}
