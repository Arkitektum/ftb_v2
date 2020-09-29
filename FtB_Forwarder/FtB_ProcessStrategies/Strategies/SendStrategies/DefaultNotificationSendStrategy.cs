using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace FtB_ProcessStrategies
{
    public class DefaultNotificationSendStrategy : SendStrategyBase
    {
        public DefaultNotificationSendStrategy( ITableStorage tableStorage, ILogger<DefaultNotificationSendStrategy> log) : base( tableStorage, log) { }

        public override ReportQueueItem Exceute(SendQueueItem sendQueueItem)
        {
            FormLogicBeingProcessed.ProcessSendStep(sendQueueItem.Receiver.Id); //Lage og persistere prefill xml

            return base.Exceute(sendQueueItem);
        }

        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Henter skjema og vedlegg for NOTIFICATION");
        }
    }
}
