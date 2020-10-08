using System;

namespace Altinn.Common.Models
{
    public class PrefillData
    {
        public string ServiceCode { get; set; }
        public string ServiceEditionCode { get; set; }
        public AltinnReceiver Receiver { get; set; }
        public string DistributionFormId { get; set; }
        public string ServiceOwnerCode { get; set; }
        public string DataFormatId { get; set; }
        public string DataFormatVersion { get; set; }
        public string XmlDataString { get; set; }
        public int DaysValid { get; set; }
        public DateTime? DueDate { get; set; }
    }
}