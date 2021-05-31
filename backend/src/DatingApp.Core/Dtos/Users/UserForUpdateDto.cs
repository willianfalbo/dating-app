using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Dtos.Users
{
    public class UserForUpdateDto
    {
        [Required]
        public string Gender { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string KnownAs { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }
    }
}