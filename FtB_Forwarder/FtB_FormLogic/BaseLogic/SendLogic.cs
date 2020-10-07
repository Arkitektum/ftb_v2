using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic
{
    public abstract class SendLogic<T> : LogicBase<T>, IFormLogic<ReportQueueItem, SendQueueItem>
    {
        protected virtual Receiver Receiver { get; set; }

        public SendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log) : base(repo, tableStorage, log)
        {
        }

        public virtual ReportQueueItem Execute(SendQueueItem sendQueueItem)
        {
            _log.LogDebug($"{GetType().Name}: Processing logic for archveReference {sendQueueItem.ArchiveReference}....");
            _log.LogDebug($"{GetType().Name}: LoadFormData for ArchiveReference {sendQueueItem.ArchiveReference}....");
            base.LoadData(sendQueueItem.ArchiveReference);

            return new ReportQueueItem() { ArchiveReference = sendQueueItem.ArchiveReference, Receiver = sendQueueItem.Receiver };

        }
    }
}
