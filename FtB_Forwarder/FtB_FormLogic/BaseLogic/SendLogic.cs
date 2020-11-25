using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public abstract class SendLogic<T> : LogicBase<T>, IFormLogic<ReportQueueItem, SendQueueItem>
    {

        protected virtual Receiver Receiver { get; set; }

        public SendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, DbUnitOfWork dbUnitOfWork) 
            : base(repo, tableStorage, log, dbUnitOfWork)
        {
        }

        public virtual async Task<ReportQueueItem> Execute(SendQueueItem sendQueueItem)
        {
            this.Receiver = sendQueueItem.Receiver;
            await base.LoadData(sendQueueItem.ArchiveReference);

            return new ReportQueueItem() { ArchiveReference = sendQueueItem.ArchiveReference, ReceiverLogPartitionKey = sendQueueItem.ReceiverLogPartitionKey, 
                                           ReceiverSequenceNumber = sendQueueItem.ReceiverSequenceNumber, Receiver = sendQueueItem.Receiver };
        }
    }
}
