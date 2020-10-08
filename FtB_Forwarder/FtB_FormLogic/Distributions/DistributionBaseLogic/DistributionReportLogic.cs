using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    public class DistributionReportLogic<T> : ReportLogic<T>
    {
        

        public DistributionReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, IEnumerable<IMessageManager> messagemanagers) : base(repo, tableStorage, log, messagemanagers)
        {
        
        }

        public override string Execute(ReportQueueItem reportQueueItem)
        {
            _log.LogDebug($"{GetType().Name}: Execute.....");
            return base.Execute(reportQueueItem);
        }
    }
}
