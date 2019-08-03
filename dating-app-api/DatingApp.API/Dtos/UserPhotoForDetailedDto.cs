using System;

namespace DatingApp.API.Dtos
{
    public class UserPhotoForDetailedDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public bool IsMain { get; set; }
    }
}