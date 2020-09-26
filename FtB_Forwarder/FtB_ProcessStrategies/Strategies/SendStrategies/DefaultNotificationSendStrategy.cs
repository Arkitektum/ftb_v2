using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;

namespace FtB_ProcessStrategies
{
    public class DefaultNotificationSendStrategy : SendStrategyBase
    {
        public DefaultNotificationSendStrategy(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage) { }

        public override ReportQueueItem Exceute(SendQueueItem sendQueueItem)
        {
            FormLogicBeingProcessed.ProcessSendStep(); //Lage og persistere prefill xml

            return base.Exceute(sendQueueItem);
        }

        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Henter skjema og vedlegg for NOTIFICATION");
        }
    }
}
