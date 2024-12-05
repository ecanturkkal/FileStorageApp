namespace FileStorageApp.Core.Dtos
{
    public class FileVersionDto
    {
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByEmail { get; set; }
    }
}
