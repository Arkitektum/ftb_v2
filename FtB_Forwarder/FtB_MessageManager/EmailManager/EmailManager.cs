using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FtB_MessageManager
{
    [MessageManagerType(Id = "EmailManager")]
    public class EmailManager : IMessageManager
    {
        private readonly IOptionsMonitor<EmailManagerSettings> _options;
        private readonly ILogger<EmailManager> _log;

        public EmailManager(IOptionsMonitor<EmailManagerSettings> options
                              , ILogger<EmailManager> log)
        {
            _options = options;
            _log = log;
        }
        public Task Send(dynamic messageElement)
        {
            throw new NotImplementedException();
        }

        private async Task Send(string emailReceiverAddress, string title, string message)
        {
            string emailSubjectPrefix = "Report from Arbeidsflyt ver. 2: ";
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