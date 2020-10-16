using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace FtB_MessageManager
{
    [MessageManagerType(Id = "SlackDistributor")]
    public class SlackManager : IMessageManager
    {
        private readonly SlackClient _client;
        private readonly IOptions<SlackManagerSettings> _options;

        public SlackManager(SlackClient client, IOptions<SlackManagerSettings> options)
        {
            _client = client;
            _options = options;
        }

        public async Task Send(string messageElement)
        {
            //string comicItemTitle = messageElement.comicItem.Safe_Title;
            //string imageLink = messageElement.comicItem.Img;
            string txt = messageElement;

            var payload = new SlackPayload() { Text = txt };

            await _client.SendMessage(_options.Value.WebHook, payload);

        }
    }
}
