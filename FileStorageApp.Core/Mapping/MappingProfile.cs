using AutoMapper;
using FileStorageApp.Core.Dtos;
using FileStorageApp.Core.Models;
using File = FileStorageApp.Core.Models.File;

namespace FileStorageApp.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Configure mapping
            CreateMap<User, UserDto>();
            CreateMap<File, FileDto>();
            CreateMap<Folder, FolderDto>();
        }
    }
}
