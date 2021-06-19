using System.Text.Json.Serialization;

namespace DatingApp.Core.Models.Slack
{
    public class PostMessageResponse
    {
        [JsonPropertyName("ok")]
        public bool Ok;

        [JsonPropertyName("channel")]
        public string Channel;

        [JsonPropertyName("ts")]
        public string Ts;

        [JsonPropertyName("message")]
        public Message Message;

        [JsonPropertyName("error")]
        public string Error;
    }

    public class Message
    {
        [JsonPropertyName("type")]
        public string Type;

        [JsonPropertyName("text")]
        public string Text;

        [JsonPropertyName("user")]
        public string User;

        [JsonPropertyName("ts")]
        public string Ts;

        [JsonPropertyName("team")]
        public string Team;
    }
}
