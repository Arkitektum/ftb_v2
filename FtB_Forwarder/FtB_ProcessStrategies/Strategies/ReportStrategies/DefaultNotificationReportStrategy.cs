﻿using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public class DefaultNotificationReportStrategy : ReportStrategyBase
    {
        private readonly IEnumerable<IMessageManager> _messageManagers;

        public DefaultNotificationReportStrategy( ITableStorage tableStorage, ILogger<DefaultNotificationReportStrategy> log, IEnumerable<IMessageManager> messageManagers)
            : base( tableStorage, log, messageManagers)
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
