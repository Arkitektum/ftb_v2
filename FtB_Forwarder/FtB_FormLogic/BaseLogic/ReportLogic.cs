using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
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
        public ReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, DbUnitOfWork dbUnitOfWork)
            : base(repo, tableStorage, log, dbUnitOfWork)
        {
        }

        public virtual void SetSubmitterReportContent(SubmittalEntity submittalEntity)
        {
        }

        protected bool AreAllReceiversReadyForReporting(ReportQueueItem reportQueueItem)
        {
            SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>(reportQueueItem.ArchiveReference, reportQueueItem.ArchiveReference);
            var totalNumberOfReceivers = submittalEntity.ReceiverCount;
            var allReceiversInSubmittal = _tableStorage.GetTableEntities<ReceiverEntity>(reportQueueItem.ArchiveReference);
            //Get number of receivers with process-stage = Done, and compare this number to the totalNumberOfReceivers
            var receiversReadyForReporting = allReceiversInSubmittal.Where(x => x.ProcessStage.Equals(Enum.GetName(typeof(ReceiverProcessStageEnum), ReceiverProcessStageEnum.ReadyForReporting))).Count();

            return receiversReadyForReporting == totalNumberOfReceivers;
        }

        protected int GetReceiverSuccessfullyNotifiedCount(ReportQueueItem reportQueueItem)
        {
            var allReceiversInSubmittal = _tableStorage.GetTableEntities<ReceiverEntity>(reportQueueItem.ArchiveReference);
            
            return allReceiversInSubmittal.Where(x => x.ProcessOutcome.Equals(Enum.GetName(typeof(ReceiverProcessOutcomeEnum), ReceiverProcessOutcomeEnum.Sent))).Count();
        }

        public virtual async Task<string> Execute(ReportQueueItem reportQueueItem)
        {
            await base.LoadData(reportQueueItem.ArchiveReference);

            return reportQueueItem.ArchiveReference;
        }
    }
}

