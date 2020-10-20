using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Ftb_DbRepository;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic
{
    public abstract class SendLogic<T> : LogicBase<T>, IFormLogic<ReportQueueItem, SendQueueItem>
    {

        protected virtual Receiver Receiver { get; set; }

        public SendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, DbUnitOfWork dbUnitOfWork) : base(repo, tableStorage, log, dbUnitOfWork)
        {
        }

        public virtual ReportQueueItem Execute(SendQueueItem sendQueueItem)
        {
            _log.LogDebug($"{GetType().Name}: Processing logic for archiveReference {sendQueueItem.ArchiveReference}....");
            _dbUnitOfWork.LogEntries.Add(new Ftb_DbModels.LogEntry(ArchiveReference, "Executing stuff.."));
            base.LoadData(sendQueueItem.ArchiveReference);

            return new ReportQueueItem() { ArchiveReference = sendQueueItem.ArchiveReference, StorageRowKey = sendQueueItem.StorageRowKey, Receiver = sendQueueItem.Receiver };

        }
    }
}
