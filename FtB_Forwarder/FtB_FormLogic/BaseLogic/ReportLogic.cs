using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic
{
    public class ReportLogic<T> : LogicBase<T>, IFormLogic<string, ReportQueueItem>, IReportLogic
    {
        public ReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ITableStorageOperations tableStorageOperations, ILogger log, DbUnitOfWork dbUnitOfWork) 
            : base(repo, tableStorage, tableStorageOperations, log, dbUnitOfWork)
        {
        }

        public virtual void SetSubmitterReportContent(SubmittalEntity submittalEntity)
        {
        }

        protected bool AreAllReceiversReadyForReporting(ReportQueueItem reportQueueItem)
        {
            SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>(reportQueueItem.ArchiveReference, reportQueueItem.ArchiveReference);
            var totalNumberOfReceivers = submittalEntity.ReceiverCount;
            string partitionKey;
            for (int i = 0; i < totalNumberOfReceivers; i++)
            {
                partitionKey = reportQueueItem.ArchiveReference + "-" + reportQueueItem.ReceiverSequenceNumber;
                if (IsReceiverReadyForReporting(partitionKey) == false)
                {
                    return false;
                }
            }

            return true;
        }

        protected int GetReceiverSuccessfullyNotifiedCount(ReportQueueItem reportQueueItem)
        {
            SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>(ArchiveReference, ArchiveReference);
            int receiverCountReadyForReporting = 0;
            string partitionKey = reportQueueItem.ArchiveReference + "-" + reportQueueItem.ReceiverSequenceNumber;
            for (int i = 0; i < submittalEntity.ReceiverCount; i++)
            {
                if (IsReceiverReadyForReporting(partitionKey))
                {
                    receiverCountReadyForReporting++;
                }
            }
            return receiverCountReadyForReporting;
        }

        private bool IsReceiverReadyForReporting(string partitionKey)
        {
            var lastReceiverStatus = _tableStorageOperations.GetReceiverLastProcessStatus(partitionKey);

            return lastReceiverStatus == ReceiverStatusEnum.ReadyForReporting;
        }
        public virtual string Execute(ReportQueueItem reportQueueItem)
        {
            base.LoadData(reportQueueItem.ArchiveReference);

            return reportQueueItem.ArchiveReference;
        }
    }
}

