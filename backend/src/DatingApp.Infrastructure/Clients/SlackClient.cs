using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Interfaces.Clients;
using DatingApp.Core.Models.Slack;
using Microsoft.Extensions.Configuration;

namespace DatingApp.Infrastructure.Clients
{
    public class SlackClient : HttpClientBase, ISlackClient
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

        public async Task<PostMessageResponse> PostChatMessageAsync(string channelName, string message)
        {
            if (string.IsNullOrWhiteSpace(channelName))
                throw new ArgumentNullException(nameof(channelName));

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            var response = await base.PostAsync<PostMessageResponse>("/chat.postMessage", new
            {
                channel = channelName,
                text = message
            });

            return response.Data;
        }
    }
}