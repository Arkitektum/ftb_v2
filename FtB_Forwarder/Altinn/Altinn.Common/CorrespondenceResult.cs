namespace Altinn.Common
{
    public class CorrespondenceResult : DistributionResult
    {
        public CorrespondenceResult() : base(DistributionComponent.Correspondence)
        {
        }

        public string CorrespondenceAltinnReceiptId { get; set; }
    }
}