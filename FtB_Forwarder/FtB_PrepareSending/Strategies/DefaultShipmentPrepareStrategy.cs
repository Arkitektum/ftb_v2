using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_PrepareSending.Strategies
{
    public class DefaultShipmentPrepareStrategy : PrepareStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the ShipmentDefaultPrepareStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultShipmentPrepareStrategy(IForm form) : base(form) { }

        protected override void CreateSubmittalDatabaseStatus(string archiveReference)
        {
            Console.WriteLine("Oppretter databasestatus for SHIPMENT");
        }

        public override List<SendQueueItem> Exceute()
        {
            _formBeingProcessed.ProcessPrepareStep();
            return null;
        }
    }
}
