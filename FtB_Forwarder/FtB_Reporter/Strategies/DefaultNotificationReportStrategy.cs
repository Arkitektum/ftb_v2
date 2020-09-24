using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Reporter.Strategies
{
    public class DefaultNotificationReportStrategy : ReportStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the NotificationDefaultReportStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultNotificationReportStrategy(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage) { }

        public override List<FinishedQueueItem> Exceute(ReportQueueItem reportQueueItem)
        {
            FormLogicBeingProcessed.ProcessReportStep();
            return null;
        }
    }
}
