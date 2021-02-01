namespace Altinn.Common.Models
{
    public class NotificationEnums
    {
        public enum NotificationChannel
        {
            None,
            Prefill,
            Correspondence,
            CorrespondenceWithPrefillEndpointValidation
        }

        public enum NotificationCarrier
        {
            None,
            Altinn,
            AltinnEmailPreferred,
            AltinnSmsPreferred,
            EmailFromDistribution,
            EmailFromDistributionOrAltinnWhenInvalidEmailAddress  // Service works by supplying an invalid direct email address when it is to run the Altinn path
        }
    }
}
