using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FtB_ProcessStrategies
{
    public abstract class SendStrategyBase : StrategyBase, IStrategy<ReportQueueItem, SendQueueItem>
    {
        protected readonly IFormDataRepo repo;
        private readonly ILogger _log;

        public SendStrategyBase(IFormDataRepo repo, ITableStorage tableStorage, ILogger log) : base(tableStorage, log)
        {
            this.repo = repo;
            _log = log;
        }

        public abstract void GetFormsAndAttachmentsFromBlobStorage();

        public virtual ReportQueueItem Exceute(SendQueueItem sendQueueItem)
        {
            FormLogicBeingProcessed.InitiateForm();

            

            return new ReportQueueItem() { ArchiveReference = sendQueueItem.ArchiveReference, Receiver = sendQueueItem.Receiver };
        }
    }
}