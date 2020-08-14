using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Distributor
{
    [DistributorType(Id = "EmailDistributor")]
    public class EmailDistributor : IDistributor
    {
        private readonly IOptionsMonitor<DistributorSettings> _options;
        public EmailDistributor(IOptionsMonitor<DistributorSettings> options)
        {
            _options = options;
        }
        public async Task Distribute(string distributionMessage)
        {
            var smtpClient = new SmtpClient(_options.CurrentValue.SMTPHost)
            {
                Port = 587,
                Credentials = new NetworkCredential(_options.CurrentValue.EmailSenderAddress, _options.CurrentValue.EmailSenderPassword),
                EnableSsl = true,
            };

            smtpClient.Send(_options.CurrentValue.EmailSenderAddress, _options.CurrentValue.EmailReceiverAddress, _options.CurrentValue.Subject, _options.CurrentValue.Message);
        }
    }
}
