using FtB_Common;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding.Strategies
{
    public class DefaultShipmentPrepareStrategy : PrepareStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the ShipmentDefaultPrepareStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultShipmentPrepareStrategy(IForm form) : base(form)
        {

        }
        protected override void CreateSubmittalDatabaseStatus(string archiveReference)
        {
            Console.WriteLine("Oppretter databasestatus for SHIPMENT");
        }

        public override void Exceute()
        {
            _formBeingProcessed.ProcessPrepareStep();
        }

        protected override void ReadReceiverInformation(string archiveReference)
        {
            Console.WriteLine("Leser mottakerinformasjon for SHIPMENT");
        }

        public void TransformSubmittalToForwardingMessage()
        {
            Console.WriteLine("Transformerer innsending til (antall) mottakere for SHIPMENT");
        }
    }
}
