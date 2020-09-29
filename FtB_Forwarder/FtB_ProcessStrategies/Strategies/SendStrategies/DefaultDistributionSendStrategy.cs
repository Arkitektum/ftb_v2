using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace FtB_ProcessStrategies
{
    public class DefaultDistributionSendStrategy : SendStrategyBase
    {
        public DefaultDistributionSendStrategy(ITableStorage tableStorage, ILogger<DefaultDistributionSendStrategy> log) : base(tableStorage, log) { }


        public override ReportQueueItem Exceute(SendQueueItem sendQueueItem)
        {
            Console.WriteLine($"DefaultDistributionSendStrategy: { FormLogicBeingProcessed.ArchiveReference }");
            FormLogicBeingProcessed.ProcessSendStep(sendQueueItem.Receiver.Id); //Lage og persistere prefill xml

            // Get prefill data generated from formlogic
            
            // Map to a specific type i.e. Prefill-type for altinn

            // Send using prefill service
            


            return base.Exceute(sendQueueItem);
        }

        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Henter skjema og vedlegg for DISTRIBUTION");
        }






    }
}
