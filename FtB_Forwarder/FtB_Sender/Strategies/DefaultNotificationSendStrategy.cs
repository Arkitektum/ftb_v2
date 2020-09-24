using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Sender.Strategies
{
    public class DefaultNotificationSendStrategy : SendStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the NotificationDefaultSendStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultNotificationSendStrategy(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage) { }

        public override List<ReportQueueItem> Exceute(SendQueueItem sendQueueItem)
        {
            FormLogicBeingProcessed.ProcessSendStep();

            return null;
        }
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
