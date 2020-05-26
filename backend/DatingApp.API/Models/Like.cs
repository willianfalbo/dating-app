namespace DatingApp.API.Models
{
    public class Like
    {
        public int LikerId { get; set; }
        public int LikeeId { get; set; }
        public virtual User Liker { get; set; }
        public virtual User Likee { get; set; }

        public Like(int likerId, int likeeId)
        {
            this.LikerId = likerId;
            this.LikeeId = likeeId;
        }
    }
}