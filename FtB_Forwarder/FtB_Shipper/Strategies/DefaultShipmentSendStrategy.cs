using FtB_Common;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding.Strategies
{
    public class DefaultShipmentSendStrategy : SendStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the ShipmentDefaultSendStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultShipmentSendStrategy(IForm form) : base(form)
        {

        }

        public override void Exceute()
        {
            _formBeingProcessed.ProcessCustomSendStep();
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
