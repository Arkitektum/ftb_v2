using Distributor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class EmailDistributorTests
    {
        private readonly IDistributor _distributor;

        public EmailDistributorTests(IOptionsMonitor<DistributorSettings> options
            , ILogger<EmailDistributor> log)
        {
            EmailDistributor ed = new EmailDistributor(options, log);
            _distributor = ed;
        }
        [Fact]
        public async Task SendEmailTest()
        {
            await _distributor.Distribute("<receiver>","<title>","<message>");

        }
    }
}
