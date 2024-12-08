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

            CreateMap<File, FileDto>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.Username));

            CreateMap<Folder, FolderDto>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.Username))
                .ForMember(dest => dest.FileCount, opt => opt.MapFrom(src => src.Files.Count))
                .ForMember(dest => dest.SubfolderCount, opt => opt.MapFrom(src => src.Subfolders.Count));

            CreateMap<Folder, FolderDetailsDto>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.Username))
                .ForMember(dest => dest.FileCount, opt => opt.MapFrom(src => src.Files.Count))
                .ForMember(dest => dest.SubfolderCount, opt => opt.MapFrom(src => src.Subfolders.Count));

            CreateMap<Share, ShareDto>()
                .ForMember(dest => dest.SharedByName, opt => opt.MapFrom(src => src.SharedBy.Username))
                .ForMember(dest => dest.SharedWithName, opt => opt.MapFrom(src => src.SharedWith.Username));
        }
    }
}
