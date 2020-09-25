using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Reporter.Strategies

{
    public class DefaultDistributionReportStrategy : ReportStrategyBase
    {
        private readonly IEnumerable<IMessageManager> _messageManagers;

        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the DistributionDefaultReportStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultDistributionReportStrategy(IFormLogic formLogic, ITableStorage tableStorage, IEnumerable<IMessageManager> messageManagers)
            : base(formLogic, tableStorage, messageManagers)
        {
            _messageManagers = messageManagers;
        }

        public override List<FinishedQueueItem> ExceuteAndReturnList(ReportQueueItem reportQueueItem)
        {
            base.ExceuteAndReturnList(reportQueueItem);
            FormLogicBeingProcessed.ProcessReportStep();
            return null;
        }
    }
}
