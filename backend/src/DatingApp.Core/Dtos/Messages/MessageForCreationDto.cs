using System;

namespace DatingApp.Core.Dtos.Messages
{
    public class MessageForCreationDto
    {
        public int RecipientId { get; set; }
        public DateTimeOffset MessageSent { get; set; }
        public string Content { get; set; }

        public MessageForCreationDto()
        {
            MessageSent = DateTimeOffset.Now;
        }
    }
}