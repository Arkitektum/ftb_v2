using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Enums
{
    public enum DistributionReceiverProcessStageEnum
    {
        Created,
        Processing,
        Processed,
        ReadyForReporting,
        Completed,
        Reported
    }
}
