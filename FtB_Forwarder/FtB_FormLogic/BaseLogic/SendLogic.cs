using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Ftb_Repositories;
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
            this.Receiver = sendQueueItem.Receiver;
            _log.LogDebug($"{GetType().Name}: Processing logic for archiveReference {sendQueueItem.ArchiveReference}....");            
            base.LoadData(sendQueueItem.ArchiveReference);

            return new ReportQueueItem() { ArchiveReference = sendQueueItem.ArchiveReference, StorageRowKey = sendQueueItem.StorageRowKey, Receiver = sendQueueItem.Receiver };
        }
    }
}
