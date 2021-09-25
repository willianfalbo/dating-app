using System.Linq;
using AutoMapper;
using DatingApp.Core.Dtos.Messages;
using DatingApp.Core.Entities;
using DatingApp.Core.Models;

namespace DatingApp.Infrastructure.Mappers.Profiles
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<MessageForCreationDto, Message>()
                .ReverseMap();

            CreateMap<Message, MessageToReturnDto>()
                .ForMember(m => m.SenderKnownAs, opt => opt.MapFrom(u => u.Sender.KnownAs))
                .ForMember(m => m.SenderPhotoUrl, opt =>
                    opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.SenderGender, opt => opt.MapFrom(u => u.Sender.Gender))
                .ForMember(m => m.RecipientKnownAs, opt => opt.MapFrom(u => u.Recipient.KnownAs))
                .ForMember(m => m.RecipientPhotoUrl, opt =>
                    opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.RecipientGender, opt => opt.MapFrom(u => u.Recipient.Gender));

            CreateMap<Paginated<MessageToReturnDto>, Paginated<Message>>()
                .ReverseMap();
        }
    }
}
