using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Distributor
{
    [DistributorType(Id = "SlackDistributor")]
    public class SlackDistributor : IDistributor
    {
        private readonly SlackClient _client;
        private readonly IOptions<SlackDistributorSettings> _options;

        public SlackDistributor(SlackClient client, IOptions<SlackDistributorSettings> options)
        {
            _client = client;
            _options = options;
        }

        public async Task Distribute(dynamic distributionElement)
        {
            string comicItemTitle = distributionElement.comicItem.Safe_Title;
            string imageLink = distributionElement.comicItem.Img;

            var payload = new SlackPayload() { Text = $"{comicItemTitle} \n{imageLink}" };

            await _client.SendMessage(_options.Value.WebHook, payload);

        }
    }
}
