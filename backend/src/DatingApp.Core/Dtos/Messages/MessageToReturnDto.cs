using System;

namespace DatingApp.Core.Dtos.Messages
{
    public class MessageToReturnDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderKnownAs { get; set; }
        public string SenderPhotoUrl { get; set; }
        public string SenderGender { get; set; }
        public int RecipientId { get; set; }
        public string RecipientKnownAs { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public string RecipientGender { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset? DateRead { get; set; }
        public DateTimeOffset MessageSent { get; set; }
    }
}