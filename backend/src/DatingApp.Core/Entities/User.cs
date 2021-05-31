using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Core.Entities
{
    // TODO: Move identity to infrastructure project
    public class User : IdentityUser<int>
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

        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Like> LikesSent { get; set; }
        public virtual ICollection<Like> LikesReceived { get; set; }
        public virtual ICollection<Message> MessagesSent { get; set; } = new Collection<Message>();
        public virtual ICollection<Message> MessagesReceived { get; set; } = new Collection<Message>();
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}