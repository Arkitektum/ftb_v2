using System.Threading.Tasks;

namespace Distributor
{
    [DistributorType(Id = "SlackDistributor")]
    public class SlackDistributor : IDistributor
    {
        private readonly SlackClient _client;

        public SlackDistributor(SlackClient client)
        {
            _client = client;
        }
        public async Task Distribute(string receiverAddress, string title, string message)
        {
            //Format slack payload
            var payload = new SlackPayload() { Text = $"{title} \n{message}" };

            await _client.SendMessage(payload);
        }
    }
}
