using System;

namespace DatingApp.API.Models
{
    public class UserPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; } // it comes from cloudinary
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}