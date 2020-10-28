namespace Altinn.Common
{
    public class DistributionResult
    {
        public DistributionResult(DistributionComponent distributionComponent)
        {
            DistributionComponent = distributionComponent;
        }
        public DistributionComponent DistributionComponent { get; }
        public string Message { get; set; }
        public DistriutionStep Step { get; set; }     
    }
}