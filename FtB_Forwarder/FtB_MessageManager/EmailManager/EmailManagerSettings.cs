using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_MessageManager
{
    public class EmailManagerSettings
    {
        public string SMTPHost { get; set; }
        public string EmailSenderAddress { get; set; }
        public string EmailSenderPassword { get; set; }
        public string EmailReceiverAddress { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
