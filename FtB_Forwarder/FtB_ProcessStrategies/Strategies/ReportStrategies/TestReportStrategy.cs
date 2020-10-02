using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ProcessStrategies
{
    public class TestReportStrategy : ReportStrategyBase
    {
        private readonly IEnumerable<IMessageManager> _messageManagers;

        public TestReportStrategy(ITableStorage tableStorage, ILogger<TestReportStrategy> log, IEnumerable<IMessageManager> messageManagers)
            : base(tableStorage, log, messageManagers)
        {
            _messageManagers = messageManagers;
        }

        public override FinishedQueueItem Exceute(ReportQueueItem reportQueueItem)
        {
            //MultipleUpTheReceiversForTheStrategy();

            return base.Exceute(reportQueueItem);
        }
    }
}
