using System.Linq;
using AutoMapper;
using DatingApp.Core.Dtos.Users;
using DatingApp.Core.Entities;
using DatingApp.Core.Helpers;

namespace DatingApp.Infrastructure.Mapper.Profiles
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge());
                });

            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge());
                });

            CreateMap<UserForUpdateDto, User>();

            CreateMap<UserForRegisterDto, User>();
        }
    }
}