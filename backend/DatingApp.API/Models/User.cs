using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Models
{
    public class User: IdentityUser<int>
    {
        public string Gender { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<UserPhoto> Photos { get; set; }
        public ICollection<Like> Likers { get; set; }
        public ICollection<Like> Likees { get; set; }
        public ICollection<Message> MessagesSent { get; set; } = new Collection<Message>();
        public ICollection<Message> MessagesReceived { get; set; } = new Collection<Message>();
        public ICollection<UserRole> UserRoles { get; set; }
    }
}