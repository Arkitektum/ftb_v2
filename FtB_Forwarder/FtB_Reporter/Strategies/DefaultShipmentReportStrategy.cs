using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Reporter.Strategies
{
    public class DefaultShipmentReportStrategy : ReportStrategyBase
    {
        private readonly IEnumerable<IMessageManager> _messageManagers;

        public DefaultShipmentReportStrategy(IFormLogic formLogic, ITableStorage tableStorage, IEnumerable<IMessageManager> messageManagers) 
            : base(formLogic, tableStorage, messageManagers)
        {
            _messageManagers = messageManagers;
        }

        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the ShipmentDefaultReportStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>

        public override List<FinishedQueueItem> ExceuteAndReturnList(ReportQueueItem reportQueueItem)
        {
            FormLogicBeingProcessed.ProcessReportStep();
            return null;
        }
    }
}
