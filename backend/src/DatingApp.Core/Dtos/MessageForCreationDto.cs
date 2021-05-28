using System;

namespace DatingApp.Core.Dtos
{
    public class MessageForCreationDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTimeOffset MessageSent { get; set; }
        public string Content { get; set; }

        public MessageForCreationDto()
        {
            MessageSent = DateTimeOffset.Now;
        }
    }
}