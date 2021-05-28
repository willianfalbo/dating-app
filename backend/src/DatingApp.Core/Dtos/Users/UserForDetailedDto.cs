using System;
using System.Collections.Generic;
using DatingApp.Core.Dtos.UserPhotos;

namespace DatingApp.Core.Dtos.Users
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public ICollection<UserPhotoForDetailedDto> Photos { get; set; }
    }
}