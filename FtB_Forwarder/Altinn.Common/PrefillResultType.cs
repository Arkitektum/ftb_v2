using System;

namespace Altinn.Common
{
    public enum DistributionStep
    {
        PayloadCreated,
        Sent,
        Failed,
        UnkownErrorOccurred,
        ReservedReportee,
        UnableToReachReceiver,
    }
}