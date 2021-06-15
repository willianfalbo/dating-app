using AutoMapper;
using DatingApp.Core.Dtos.Photos;
using DatingApp.Core.Entities;

namespace DatingApp.Infrastructure.Mappers.Profiles
{
    public class PhotoMappingProfile : Profile
    {
        public PhotoMappingProfile()
        {
            CreateMap<Photo, PhotoForDetailedDto>();

            CreateMap<PhotoForCreationDto, Photo>();

            CreateMap<Photo, PhotoToReturnDto>();
        }
    }
}