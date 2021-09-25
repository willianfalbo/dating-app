using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Dtos.Users
{
    public class UserForRegisterDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "You must specify a username between 4 and 50 characters.")]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters.")]
        public string Password { get; set; }

        [Required]
        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set
            {
                value = value?.ToLower()?.Trim();
                _gender = (value == "male" || value == "female") ? value : "unknown";
            }
        }

        [Required]
        public string KnownAs { get; set; }

        [Required]
        public DateTimeOffset DateOfBirth { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }
    }
}