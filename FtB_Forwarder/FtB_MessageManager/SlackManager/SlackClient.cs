using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FtB_MessageManager
{
    public class SlackClient
    {
        public HttpClient Client { get; }
        public SlackClient(HttpClient client)
        {
            Client = client;
        }
        
        public async Task SendMessage(string webHook, SlackPayload payload)
        {
            var json = JsonSerializer.Serialize(payload);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            Client.BaseAddress = new Uri(webHook);
            await Client.PostAsync("", stringContent);
        }
    }
}
