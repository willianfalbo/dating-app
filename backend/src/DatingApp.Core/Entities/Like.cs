namespace DatingApp.Core.Entities
{
    public class Like
    {
        public virtual User Sender { get; set; }
        public int SenderId { get; set; }
        public virtual User Receiver { get; set; }
        public int ReceiverId { get; set; }

        public Like(int senderId, int receiverId)
        {
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
        }
    }
}