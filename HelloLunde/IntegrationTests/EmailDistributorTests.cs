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

        public EmailDistributorTests(IOptionsMonitor<EmailDistributorSettings> options
            , ILogger<EmailDistributor> log)
        {
            EmailDistributor ed = new EmailDistributor(options, log);
            _distributor = ed;
        }
        [Fact]
        public async Task SendEmailTest()
        {
            dynamic myDynamic = new System.Dynamic.ExpandoObject();

            myDynamic.emailTo = "<receiver>";
            myDynamic.comicItem.Safe_Title = "<title>";
            myDynamic.comicItem.Transcript = "<message>";


            await _distributor.Distribute(myDynamic);

        }
    }
}
