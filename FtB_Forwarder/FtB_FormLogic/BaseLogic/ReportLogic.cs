using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public class ReportLogic<T> : LogicBase<T>, IFormLogic<string, ReportQueueItem>, IReportLogic
    {
        protected readonly IBlobOperations _blobOperations;
        private readonly int BLOB_CONTAINER_LEASE_DURATION_MAX = 60;
        public ReportLogic(IFormDataRepo repo, ITableStorage tableStorage, IBlobOperations blobOperations, ILogger log, DbUnitOfWork dbUnitOfWork)
            : base(repo, tableStorage, log, dbUnitOfWork)
        {
            _blobOperations = blobOperations;
        }

        public virtual void SetSubmitterReportContent(SubmittalEntity submittalEntity)
        {
        }

        protected async Task<bool> ReadyForSubmittalReporting(ReportQueueItem reportQueueItem)
        {
            if (await AllReceiversReadyForReporting(reportQueueItem))
            {
                _log.LogDebug($"{GetType().Name}: Trying to set blob lease by ArchiveReference {reportQueueItem.ArchiveReference}-{reportQueueItem.ReceiverSequenceNumber}....");
                return await SetReportingFlagForSubmittal(reportQueueItem.ArchiveReference.ToLower());
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// If method returns false, then this process was not able to acquire a lase on the container for this submittal.
        /// This is due to a preceeding process already ha acqired the lease, and is therefore sending the submittal receipt
        /// </summary>
        /// <returns></returns>
        private async Task<bool> SetReportingFlagForSubmittal(string containerName)
        {
            return await _blobOperations.AcquireContainerLease(containerName, BLOB_CONTAINER_LEASE_DURATION_MAX);
        }

        private async Task<bool> AllReceiversReadyForReporting(ReportQueueItem reportQueueItem)
        {
            SubmittalEntity submittalEntity = await _tableStorage.GetTableEntity<SubmittalEntity>(reportQueueItem.ArchiveReference, reportQueueItem.ArchiveReference);
            var totalNumberOfReceivers = submittalEntity.ReceiverCount;
            var allReceiversInSubmittal = await _tableStorage.GetTableEntities<ReceiverEntity>(reportQueueItem.ArchiveReference);
            //Get number of receivers with process-stage = Done, and compare this number to the totalNumberOfReceivers
            var receiversReadyForReporting = allReceiversInSubmittal.Where(x => x.ProcessStage.Equals(Enum.GetName(typeof(ReceiverProcessStageEnum), ReceiverProcessStageEnum.ReadyForReporting))).Count();

            return receiversReadyForReporting == totalNumberOfReceivers;
        }

        protected async Task<int> GetReceiverSuccessfullyNotifiedCount(ReportQueueItem reportQueueItem)
        {
            var allReceiversInSubmittal = await _tableStorage.GetTableEntities<ReceiverEntity>(reportQueueItem.ArchiveReference);

            return allReceiversInSubmittal.Where(x => x.ProcessOutcome.Equals(Enum.GetName(typeof(ReceiverProcessOutcomeEnum), ReceiverProcessOutcomeEnum.Sent))).Count();
        }

        public virtual async Task<string> Execute(ReportQueueItem reportQueueItem)
        {
            _log.LogDebug($"{GetType().Name}: 'Execute' for ArchiveReference {reportQueueItem.ArchiveReference}-{reportQueueItem.ReceiverSequenceNumber}....");
            await base.LoadData(reportQueueItem.ArchiveReference);

            return reportQueueItem.ArchiveReference;
        }
    }
}

