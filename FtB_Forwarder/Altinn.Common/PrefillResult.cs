using System;

namespace Altinn.Common
{
    public class PrefillResult : DistributionResult
    {
        public PrefillResult() : base(DistributionComponent.Prefill)
        {
        }

        public string PrefillAltinnReceiptId { get; set; }
        public DateTime PrefillAltinnReceivedTime { get; set; }
    }
}