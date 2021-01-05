using FtB_Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Exceptions
{
    public class DistributionSendExeception : Exception
    {
        public string Text { get; set; }
        public string InitialExternalSystemReference { get; set; }
        public string ExternalSystemReference { get; set; }
        public Guid DistributionFormReferenceId { get; set; }

        public DistributionSendExeception(string initialExternalSystemReference, string externalSystemReference, Guid distributionFormReferenceId, string text)
        {
            Text = text;
            InitialExternalSystemReference = initialExternalSystemReference;
            ExternalSystemReference = externalSystemReference;
            DistributionFormReferenceId = distributionFormReferenceId;
        }
    }
}
