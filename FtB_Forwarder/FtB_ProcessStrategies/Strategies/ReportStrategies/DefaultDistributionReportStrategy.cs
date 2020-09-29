﻿using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FtB_ProcessStrategies

{
    public class DefaultDistributionReportStrategy : ReportStrategyBase
    {
        private readonly IEnumerable<IMessageManager> _messageManagers;

        public DefaultDistributionReportStrategy(IFormLogic formLogic, ITableStorage tableStorage, ILogger log, IEnumerable<IMessageManager> messageManagers)
            : base(formLogic, tableStorage, log, messageManagers)
        {
            _messageManagers = messageManagers;
        }

        public override FinishedQueueItem Exceute(ReportQueueItem reportQueueItem)
        {
            base.Exceute(reportQueueItem);
            FormLogicBeingProcessed.ProcessReportStep();
            return null;
        }
    }
}
