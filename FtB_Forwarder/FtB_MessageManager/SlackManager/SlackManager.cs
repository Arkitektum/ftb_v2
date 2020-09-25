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

        public async Task Send(dynamic messageElement)
        {
            string comicItemTitle = messageElement.comicItem.Safe_Title;
            string imageLink = messageElement.comicItem.Img;

            var payload = new SlackPayload() { Text = $"{comicItemTitle} \n{imageLink}" };

            await _client.SendMessage(_options.Value.WebHook, payload);

        }
    }
}
