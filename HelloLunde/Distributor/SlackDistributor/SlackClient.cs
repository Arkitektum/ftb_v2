using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Distributor
{
    public class SlackClient
    {
        public HttpClient Client { get; }
        public SlackClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://hooks.slack.com/services/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");
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
