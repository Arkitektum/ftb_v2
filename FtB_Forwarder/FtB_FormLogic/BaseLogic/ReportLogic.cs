using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                partitionKey = reportQueueItem.ArchiveReference + "-" + i;
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
            int receiverSuccessfullyNotified = 0;
            //string partitionKey = reportQueueItem.ArchiveReference + "-" + reportQueueItem.ReceiverSequenceNumber;
            for (int i = 0; i < submittalEntity.ReceiverCount; i++)
            {
                string partitionKey = reportQueueItem.ArchiveReference + "-" + i;

                if (IsReceiverSuccessfullyNotified(partitionKey))
                {
                    receiverSuccessfullyNotified++;
                }
            }
            return receiverSuccessfullyNotified;
        }

        private bool IsReceiverReadyForReporting(string partitionKey)
        {
            var lastReceiverStatus = _tableStorageOperations.GetReceiverLastProcessStatus(partitionKey);

            return lastReceiverStatus == ReceiverStatusEnum.ReadyForReporting;
        }

        private bool IsReceiverSuccessfullyNotified(string partitionKey)
        {
            var lastReceiverStatus = _tableStorageOperations.GetReceiverLastProcessStatus(partitionKey);
            var xx = _tableStorage.GetRowsFromPartitionKey<ReceiverEntity>(partitionKey);
            return xx.Any(x => x.Status.Equals(Enum.GetName(typeof(ReceiverStatusEnum), ReceiverStatusEnum.CorrespondenceSent)));
        }


        public virtual async Task<string> Execute(ReportQueueItem reportQueueItem)
        {
            await base.LoadData(reportQueueItem.ArchiveReference);

            return reportQueueItem.ArchiveReference;
        }
    }
}

