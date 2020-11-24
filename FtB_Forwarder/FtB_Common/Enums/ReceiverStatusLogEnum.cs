using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Enums
{
    public enum ReceiverStatusLogEnum
    {
        Created,
        PrefillCreated,
        PrefillPersisted,
        PrefillSent,
        PrefillSendingFailed,
        CorrespondenceCreated,
        CorrespondenceSendingFailed,
        CorrespondenceSent,
        ReservedReportee,
        ReadyForReporting,
        Completed
    }
}
