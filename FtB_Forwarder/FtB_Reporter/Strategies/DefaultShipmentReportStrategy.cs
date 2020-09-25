using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Reporter.Strategies
{
    public class DefaultShipmentReportStrategy : ReportStrategyBase
    {
        private readonly IEnumerable<IMessageManager> _messageManagers;

        public DefaultShipmentReportStrategy(IFormLogic formLogic, ITableStorage tableStorage, IEnumerable<IMessageManager> messageManagers, ILogger log) 
            : base(formLogic, tableStorage, messageManagers, log)
        {
            _messageManagers = messageManagers;
        }

        public override List<FinishedQueueItem> ExceuteAndReturnList(ReportQueueItem reportQueueItem)
        {
            FormLogicBeingProcessed.ProcessReportStep();
            return null;
        }
    }
}
