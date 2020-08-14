using System;
using System.Collections.Generic;
using System.Text;

namespace Distributor
{
    public class DistributorSettings
    {
        public string DistributorType { get; set; }
        public string SMTPHost { get; set; }
        public string EmailSenderAddress { get; set; }
        public string EmailSenderPassword { get; set; }
        public string EmailReceiverAddress { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
