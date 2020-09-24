using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Sender.Strategies
{
    public class DefaultShipmentSendStrategy : SendStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the ShipmentDefaultSendStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultShipmentSendStrategy(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage) { }

        public override List<ReportQueueItem> Exceute(SendQueueItem sendQueueItem)
        {
            FormLogicBeingProcessed.ProcessSendStep();
            return null;
        }
        public override void ForwardToReceiver()
        {
            Console.WriteLine("Sender skjema til SHIPMENT");
        }

        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Ikke implementert ennå....");
            //Console.WriteLine("Henter skjema og vedlegg for SHIPMENT");
        }
    }
}
