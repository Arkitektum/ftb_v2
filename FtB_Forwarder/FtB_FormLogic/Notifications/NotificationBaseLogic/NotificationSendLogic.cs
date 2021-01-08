using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Ftb_DbModels;
using Ftb_Repositories;
using Ftb_Repositories.HttpClients;
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
                                     DbUnitOfWork dbUnitOfWork, FileDownloadStatusHttpClient fileDownloadHttpClient)
            : base(repo, tableStorage, log, dbUnitOfWork, fileDownloadHttpClient)
        { }

        public override async Task<ReportQueueItem> ExecuteAsync(SendQueueItem sendQueueItem)
        {
            _log.LogDebug("_dbUnitOfWork hash {0}", _dbUnitOfWork.GetHashCode());
            await base.ExecuteAsync(sendQueueItem);

            var returnReportQueueItem = new ReportQueueItem()
            {
                ArchiveReference = sendQueueItem.ArchiveReference,
                ReceiverLogPartitionKey = sendQueueItem.ReceiverLogPartitionKey,
                ReceiverSequenceNumber = sendQueueItem.ReceiverSequenceNumber,
                Sender = sendQueueItem.Sender,
                Receiver = sendQueueItem.Receiver
            };
            
            return returnReportQueueItem;
        }
    }
}
