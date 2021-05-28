using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Core.Dtos.UserPhotos
{
    public class UserPhotoForCreationDto
    {
        public UserPhotoForCreationDto()
        {
            DateAdded = DateTimeOffset.Now;
        }

        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateAdded { get; }
    }
}