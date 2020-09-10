using FtB_CommonModel.Factories;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding
{
    public class NotificationExecuteForwarder : ExceuteForwarding, IExecute
    {
        public override void ForwardToReceiver()
        {
            Console.WriteLine("Sender skjema til NOTIFICATION");
        }

        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Henter skjema og vedlegg for NOTIFICATION");
        }
    }
}
