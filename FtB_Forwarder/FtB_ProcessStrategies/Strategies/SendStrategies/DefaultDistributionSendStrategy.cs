using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace FtB_ProcessStrategies
{
    public class DefaultDistributionSendStrategy : SendStrategyBase
    {
        public DefaultDistributionSendStrategy(IFormLogic formLogic, ITableStorage tableStorage, ILogger log) : base(formLogic, tableStorage, log) { }

        public override ReportQueueItem Exceute(SendQueueItem sendQueueItem)
        {
            Console.WriteLine($"DefaultDistributionSendStrategy: { FormLogicBeingProcessed.ArchiveReference }");
            FormLogicBeingProcessed.ProcessSendStep(); //Lage og persistere prefill xml
            
            return base.Exceute(sendQueueItem);
        }

        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Henter skjema og vedlegg for DISTRIBUTION");
        }
    }
}
