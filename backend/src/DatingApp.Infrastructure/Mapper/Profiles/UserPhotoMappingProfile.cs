using AutoMapper;
using DatingApp.Core.Dtos;
using DatingApp.Core.Entities;

namespace DatingApp.Infrastructure.Mapper.Profiles
{
    public class UserPhotoMappingProfile : Profile
    {
        public UserPhotoMappingProfile()
        {
            CreateMap<UserPhoto, UserPhotoForDetailedDto>();

            CreateMap<UserPhotoForCreationDto, UserPhoto>();

            CreateMap<UserPhoto, UserPhotoForReturnDto>();
        }
    }
}