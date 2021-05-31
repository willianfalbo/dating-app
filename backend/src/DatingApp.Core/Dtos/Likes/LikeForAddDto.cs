using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Dtos.Likes
{
    public class LikeForAddDto
    {
        [Required]
        public int ReceiverId { get; set; }
    }
}