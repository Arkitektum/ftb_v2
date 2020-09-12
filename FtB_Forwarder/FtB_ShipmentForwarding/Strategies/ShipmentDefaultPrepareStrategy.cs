using FtB_Common;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding.Strategies
{
    public class ShipmentDefaultPrepareStrategy : PrepareStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the ShipmentDefaultPrepareStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public ShipmentDefaultPrepareStrategy(IForm form) : base(form)
        {

        }
        protected override void CreateSubmittalDatabaseStatus(string archiveReference)
        {
            Console.WriteLine("Oppretter databasestatus for SHIPMENT");
        }

        public override void Exceute()
        {
            throw new NotImplementedException();
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
