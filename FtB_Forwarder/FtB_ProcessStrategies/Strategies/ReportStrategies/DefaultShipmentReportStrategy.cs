using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public class DefaultShipmentReportStrategy : ReportStrategyBase
    {
        private readonly IEnumerable<IMessageManager> _messageManagers;

        public DefaultShipmentReportStrategy(IFormLogic formLogic, ITableStorage tableStorage, ILogger log, IEnumerable<IMessageManager> messageManagers) 
            : base(formLogic, tableStorage, log, messageManagers)
        {
            _messageManagers = messageManagers;
        }

        public override FinishedQueueItem Exceute(ReportQueueItem reportQueueItem)
        {
            FormLogicBeingProcessed.ProcessReportStep();
            return null;
        }
    }
}
