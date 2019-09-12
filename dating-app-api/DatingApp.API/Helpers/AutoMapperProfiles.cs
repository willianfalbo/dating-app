using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt => {
                    opt.ResolveUsing(src => src.DateOfBirth.CalculateAge());
                });
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt => {
                    opt.ResolveUsing(src => src.DateOfBirth.CalculateAge());
                });
            CreateMap<UserPhoto, UserPhotoForDetailedDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<UserPhotoForCreationDto, UserPhoto>();
            CreateMap<UserPhoto, UserPhotoForReturnDto>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();

            CreateMap<Message, MessageToReturnDto>()
                .ForMember(m => m.SenderKnownAs, opt => 
                    opt.MapFrom(u => u.Sender.KnownAs))
                .ForMember(m => m.SenderPhotoUrl, opt => 
                    opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.SenderGender, opt => 
                    opt.MapFrom(u => u.Sender.Gender))
                .ForMember(m => m.RecipientKnownAs, opt => 
                    opt.MapFrom(u => u.Recipient.KnownAs))
                .ForMember(m => m.RecipientPhotoUrl, opt => 
                    opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.RecipientGender, opt => 
                    opt.MapFrom(u => u.Recipient.Gender));
        }
    }
}