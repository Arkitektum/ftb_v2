using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    public class DistributionReportLogic<T> : ReportLogic<T>
    {
        private readonly ILogger _log;

        public DistributionReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log) : base(repo, tableStorage, log)
        {
            _log = log;
        }

        public override string Execute(ReportQueueItem reportQueueItem)
        {
            _log.LogInformation("Jau");
            return base.Execute(reportQueueItem);
        }
    }
}
