using System;
using System.Linq;
using AutoMapper;
using DatingApp.Core.Dtos.Users;
using DatingApp.Core.Entities;
using DatingApp.Core.Helpers;
using DatingApp.Core.Models;

namespace DatingApp.Infrastructure.Mappers.Profiles
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

            CreateMap<UserForRegisterDto, User>()
                .ForMember(dest => dest.Created, opt => { opt.MapFrom(src => DateTimeOffset.Now); })
                .ForMember(dest => dest.LastActive, opt => { opt.MapFrom(src => DateTimeOffset.Now); });

            CreateMap<Paginated<UserForListDto>, Paginated<User>>()
                .ReverseMap();
        }
    }
}
