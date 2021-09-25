using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Interfaces.Services;
using DatingApp.Core.Models.Slack;
using Microsoft.Extensions.Configuration;

namespace DatingApp.Infrastructure.Clients
{
    public class SlackClient : HttpClientBase, ISlackService
    {
        private readonly IConfiguration _configuration;

        public SlackClient(IConfiguration configuration)
            : base(
                configuration["Slack:BaseUrl"],
                new Dictionary<string, string>
                {
                    { "Content-Type", "application/json; charset=utf-8" },
                    { "Content", "application/json; charset=utf-8" },
                    { "Authorization", $"Bearer {configuration["Slack:Token"]}" },
                }
            )
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc />
        public async Task<PostMessageResponse> PostChatMessageAsync(string channelName, string message, bool ignoreErrors = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(channelName))
                    throw new ArgumentNullException(nameof(channelName));

                if (string.IsNullOrWhiteSpace(message))
                    throw new ArgumentNullException(nameof(message));

                var response = await PostAsync<PostMessageResponse>("/chat.postMessage", new
                {
                    channel = channelName,
                    text = message
                });

                if (!response.IsSuccessful || !response.Data.Ok)
                    throw new Exception($"Error when posting slack message. Details: {response.Data.Error}");

                return response.Data;
            }
            catch (Exception ex)
            {
                // TODO: Log errors manually. E.g. https://serilog.net/

                if (ignoreErrors)
                    return new PostMessageResponse() { Ok = false, Error = ex.Message };

                throw ex;
            }
        }
    }
}
