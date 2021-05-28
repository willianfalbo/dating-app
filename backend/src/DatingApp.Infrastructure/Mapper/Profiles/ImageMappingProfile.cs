using AutoMapper;
using Models = DatingApp.Core.Models.Image;
using Cloudinary = CloudinaryDotNet.Actions;

namespace DatingApp.Infrastructure.Mapper.Profiles
{
    public class ImageMappingProfile : Profile
    {
        public ImageMappingProfile()
        {
            CreateMap<Cloudinary.ImageUploadResult, Models.UploadedImageResult>()
                .ForMember(m => m.Url, opt => opt.MapFrom(c => c.Url.ToString()));

            CreateMap<Cloudinary.DeletionResult, Models.DeletedFileResult>();
        }
    }
}