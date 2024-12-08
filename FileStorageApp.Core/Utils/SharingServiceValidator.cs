using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Exceptions;
using FileStorageApp.Core.Models;

namespace FileStorageApp.Core.Utils
{
    public class SharingServiceValidator
    {
        public static void ValidateCreateShareRequest(CreateShareRequestDto shareRequest)
        {
            if (!Enum.IsDefined(typeof(ResourceType), shareRequest.ResourceType))
                throw new FileStorageException($"Invalid resource type.");

            if (!Enum.IsDefined(typeof(SharePermission), shareRequest.Permission))
                throw new FileStorageException($"Invalid share permission.");
        }
    }
}
