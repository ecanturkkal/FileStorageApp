namespace FileStorageApp.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(string userId);
    }
}
