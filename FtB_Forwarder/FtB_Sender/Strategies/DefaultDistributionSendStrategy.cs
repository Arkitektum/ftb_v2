    using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Sender.Strategies
{
    public class DefaultDistributionSendStrategy : SendStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the DistributionDefaultSendStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultDistributionSendStrategy(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage) { }

        public override List<ReportQueueItem> Exceute(SendQueueItem sendQueueItem)
        {
            //FormLogicBeingProcessed.ProcessSendStep();
            base.Exceute(sendQueueItem);
            Console.WriteLine($"DefaultDistributionSendStrategy: { FormLogicBeingProcessed.ArchiveReference }");
            List<ReportQueueItem> reportQueueItems = new List<ReportQueueItem>();
            reportQueueItems.Add(new ReportQueueItem() { ArchiveReference = ArchiveReference, Receivers = FormLogicBeingProcessed.Receivers });
            return reportQueueItems;
        }
        public override void ForwardToReceiver()
        {
            Console.WriteLine("Sender skjema til DISTRIBUTION");
        }

        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Henter skjema og vedlegg for DISTRIBUTION");
        }
    }
}
