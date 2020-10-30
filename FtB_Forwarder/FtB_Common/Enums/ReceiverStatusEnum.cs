using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common
{
    public enum ReceiverStatusEnum
    {
        Created,
        PrefillCreated,
        PrefillPersisted,
        PrefillSent,
        PrefillSendingFailed,
        CorrespondenceCreated,
        CorrespondenceSendingFailed,
        CorrespondenceSent,
        DigitalDisallowment,
        ReadyForReporting
    }
}
