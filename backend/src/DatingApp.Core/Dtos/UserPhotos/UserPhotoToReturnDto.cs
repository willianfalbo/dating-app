using System;

namespace DatingApp.Core.Dtos.UserPhotos
{
    public class UserPhotoToReturnDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
    }
}