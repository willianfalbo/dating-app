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
    }

    public class BotProfile
    {
        [JsonPropertyName("id")]
        public string Id;

        [JsonPropertyName("deleted")]
        public bool Deleted;

        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("updated")]
        public int Updated;

        [JsonPropertyName("app_id")]
        public string AppId;

        [JsonPropertyName("team_id")]
        public string TeamId;
    }

    public class Message
    {
        [JsonPropertyName("bot_id")]
        public string BotId;

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

        [JsonPropertyName("bot_profile")]
        public BotProfile BotProfile;
    }
}
