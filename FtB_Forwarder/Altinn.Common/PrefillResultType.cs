using System;

namespace Altinn.Common
{
    public enum DistriutionStep
    {
        PayloadCreated,
        Sent,
        Failed,
        UnkownErrorOccurred,
        ReservedReportee,
        UnableToReachReceiver,
    }
}