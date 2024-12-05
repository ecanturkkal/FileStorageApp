namespace FileStorageApp.Core.Utils
{
    public static class Utils
    {
        public static string GenerateUniqueBlobPath(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return $"{Guid.NewGuid()}{extension}";
        }
    }
}
