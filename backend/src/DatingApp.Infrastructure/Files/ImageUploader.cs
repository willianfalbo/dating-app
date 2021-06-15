using System;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Core.Interfaces.Files;
using DatingApp.Core.Interfaces.Mappers;
using DatingApp.Core.Models.Files;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Infrastructure.Files
{
    public class ImageUploader : IImageUploader
    {
        private readonly Cloudinary _cloudinary;
        private readonly IClassMapper _mapper;

        public ImageUploader(Cloudinary cloudinary, IClassMapper mapper)
        {
            _cloudinary = cloudinary ?? throw new ArgumentNullException(nameof(cloudinary));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UploadedFileResult> UploadAsync(IFormFile file)
        {
            var result = new ImageUploadResult();

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                };

                result = await _cloudinary.UploadAsync(uploadParams);
            }

            return _mapper.To<UploadedFileResult>(result);
        }

        public async Task<DeletedFileResult> DeleteAsync(string publicId)
        {
            var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));

            return _mapper.To<DeletedFileResult>(result);
        }
    }
}