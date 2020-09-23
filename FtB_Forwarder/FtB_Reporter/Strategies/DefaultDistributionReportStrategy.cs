using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Reporter.Strategies

{
    public class DefaultDistributionReportStrategy : ReportStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the DistributionDefaultReportStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultDistributionReportStrategy(IFormLogic formLogic) : base(formLogic) { }

        public override List<FinishedQueueItem> Exceute()
        {
            FormLogicBeingProcessed.ProcessReportStep();
            return null;
        }
    }
}
