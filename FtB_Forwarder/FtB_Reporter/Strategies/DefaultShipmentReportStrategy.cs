using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Reporter.Strategies
{
    public class DefaultShipmentReportStrategy : ReportStrategyBase
    {

        public DefaultShipmentReportStrategy(IFormLogic formLogic) : base(formLogic) { }

        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the ShipmentDefaultReportStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
   
        public override List<FinishedQueueItem> Exceute()
        {
            FormLogicBeingProcessed.ProcessReportStep();
            return null;
        }
    }
}
