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
        

        private readonly IEnumerable<IMessageManager> _messageManagers;
        public DistributionReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log) : base(repo, tableStorage, log)
        {
        
        }

        public override FinishedQueueItem Execute(ReportQueueItem reportQueueItem)
        {
            _log.LogInformation("Jau");
            return base.Execute(reportQueueItem);
        }
    }
}
