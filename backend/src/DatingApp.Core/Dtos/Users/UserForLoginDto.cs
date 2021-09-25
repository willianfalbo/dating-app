using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Dtos.Users
{
    public class UserForLoginDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "You must specify a username between 4 and 50 characters.")]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters.")]
        public string Password { get; set; }
    }
}