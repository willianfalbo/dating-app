
using System.Threading.Tasks;
using DatingApp.Core.Models.Image;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Core.Interfaces
{
    public interface IImageUploader
    {
        Task<UploadedImageResult> UploadAsync(IFormFile file);
        Task<DeletedFileResult> DeleteAsync(string publicId);
    }
}