using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.BusinessModels
{
    public class SubmitterReport
    {
        public Receiver Receiver { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
