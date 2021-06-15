using System.Threading.Tasks;
using DatingApp.Core.Models.Files;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Core.Interfaces.Files
{
    public interface IImageUploader
    {
        Task<UploadedFileResult> UploadAsync(IFormFile file);
        Task<DeletedFileResult> DeleteAsync(string publicId);
    }
}