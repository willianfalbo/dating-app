using System.Threading.Tasks;
using DatingApp.Core.Models.Slack;

namespace DatingApp.Core.Interfaces.Services
{
    public interface ISlackService
    {
        /// <summary>
        /// Post a slack message.
        /// </summary>
        /// <param name="channelName">Channel name.</param>
        /// <param name="message">Message to send.</param>
        /// <param name="ignoreErrors">If yes, the method will ignore any errors if it occurs. Otherwise, an exception will be thrown.</param>
        /// <returns></returns>
        Task<PostMessageResponse> PostChatMessageAsync(string channelName, string message, bool ignoreErrors = false);
    }
}
