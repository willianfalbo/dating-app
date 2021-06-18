using System.Threading.Tasks;
using DatingApp.Core.Models.Slack;

namespace DatingApp.Core.Interfaces.Clients
{
    public interface ISlackClient
    {
        Task<PostMessageResponse> PostChatMessageAsync(string channelName, string message);
    }
}