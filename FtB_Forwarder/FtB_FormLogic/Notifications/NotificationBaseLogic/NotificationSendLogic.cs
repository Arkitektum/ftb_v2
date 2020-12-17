using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Ftb_DbModels;
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

            returnReportQueueItem = new ReportQueueItem()
            {
                ArchiveReference = sendQueueItem.ArchiveReference,
                ReceiverLogPartitionKey = sendQueueItem.ReceiverLogPartitionKey,
                ReceiverSequenceNumber = sendQueueItem.ReceiverSequenceNumber,
                Sender = sendQueueItem.Sender,
                Receiver = sendQueueItem.Receiver
            };
            
            //TODO: Flytte til prepare funskjon?

            var distributionId = GetHovedinnsendingsNummer();
            var distributionForm = await _dbUnitOfWork.DistributionForms.Get(distributionId);

            distributionForm.Signed = DateTime.Now;
            distributionForm.DistributionStatus = DistributionStatus.signed;
            distributionForm.SignedArchiveReference = sendQueueItem.ArchiveReference.ToUpper();
            
            await _dbUnitOfWork.DistributionForms.Update(distributionForm.InitialArchiveReference, distributionId, distributionForm);

            return returnReportQueueItem;
        }
        protected abstract Guid GetHovedinnsendingsNummer();

    }
}
