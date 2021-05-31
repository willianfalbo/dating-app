using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Core.Dtos.Photos
{
    public class PhotoForCreationDto
    {
        public PhotoForCreationDto()
        {
            DateAdded = DateTimeOffset.Now;
        }

        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateAdded { get; }
    }
}