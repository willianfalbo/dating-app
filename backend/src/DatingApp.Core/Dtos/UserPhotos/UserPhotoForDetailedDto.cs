using System;

namespace DatingApp.Core.Dtos.UserPhotos
{
    public class UserPhotoForDetailedDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public bool IsMain { get; set; }
        public bool IsApproved { get; set; }
    }
}