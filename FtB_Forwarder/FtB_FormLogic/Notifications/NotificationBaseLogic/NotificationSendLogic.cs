using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public abstract class NotificationSendLogic<T> : SendLogic<T>
    {
        public NotificationSendLogic(IFormDataRepo repo,
                                     ITableStorage tableStorage,
                                     ILogger log,
                                     DbUnitOfWork dbUnitOfWork)
            : base(repo, tableStorage, log, dbUnitOfWork)
        { }

        public override async Task<ReportQueueItem> ExecuteAsync(SendQueueItem sendQueueItem)
        {
            _log.LogDebug("_dbUnitOfWork hash {0}", _dbUnitOfWork.GetHashCode());
            var returnReportQueueItem = await base.ExecuteAsync(sendQueueItem);

            return returnReportQueueItem;
        }

    }
}
