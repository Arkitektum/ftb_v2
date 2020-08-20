using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Distributor
{
    public class SlackClient
    {
        private readonly IOptions<SlackSettings> _options;

        public HttpClient Client { get; }
        public SlackClient(HttpClient client, IOptions<SlackSettings> options)
        {
            _options = options;
            client.BaseAddress = new Uri(_options.Value.WebHook);
            Client = client;
        }
        
        public async Task SendMessage(SlackPayload payload)
        {
            var json = JsonSerializer.Serialize(payload);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            await Client.PostAsync("", stringContent);
        }
    }
}
