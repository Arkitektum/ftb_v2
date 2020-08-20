using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Distributor
{
    [DistributorType(Id = "EmailDistributor")]
    public class EmailDistributor : IDistributor
    {
        private readonly IOptionsMonitor<EmailDistributorSettings> _options;
        private readonly ILogger _log;
        public EmailDistributor(IOptionsMonitor<EmailDistributorSettings> options
                              , ILogger<EmailDistributor> log)
        {
            _options = options;
            _log = log;
        }
        public async Task Distribute(string emailReceiverAddress, string title, string message)
        {
            string emailSubjectPrefix = "Test from Azure functions: ";
            string receiver = emailReceiverAddress != null && emailReceiverAddress.Trim().Length > 0 
                            ? emailReceiverAddress : _options.CurrentValue.EmailReceiverAddress;

            _log.LogInformation($"Distribute message: emailReceiverAddress: {emailReceiverAddress}");
            _log.LogInformation("Distribute message: host {0}, sender {1}, receiver {2}, title {3}"
                                        , _options.CurrentValue.SMTPHost
                                        , _options.CurrentValue.EmailSenderAddress
                                        , receiver
                                        , title);

            var smtpClient = new SmtpClient(_options.CurrentValue.SMTPHost)
            {
                Port = 587,
                Credentials = new NetworkCredential(_options.CurrentValue.EmailSenderAddress, _options.CurrentValue.EmailSenderPassword),
                EnableSsl = true,
            };

            smtpClient.Send(_options.CurrentValue.EmailSenderAddress, receiver, emailSubjectPrefix + title, message);
        }
    }
}
