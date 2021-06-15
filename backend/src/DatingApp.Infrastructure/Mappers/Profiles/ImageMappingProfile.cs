using AutoMapper;
using Models = DatingApp.Core.Models.Files;
using Cloudinary = CloudinaryDotNet.Actions;

namespace DatingApp.Infrastructure.Mappers.Profiles
{
    public class ImageMappingProfile : Profile
    {
        public ImageMappingProfile()
        {
            CreateMap<Cloudinary.ImageUploadResult, Models.UploadedFileResult>()
                .ForMember(m => m.Url, opt => opt.MapFrom(c => c.Url.ToString()));

            CreateMap<Cloudinary.DeletionResult, Models.DeletedFileResult>();
        }
    }
}