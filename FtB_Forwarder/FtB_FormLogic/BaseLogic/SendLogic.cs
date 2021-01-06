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
    public abstract class SendLogic<T> : LogicBase<T>, IFormLogic<ReportQueueItem, SendQueueItem>
    {

        protected virtual Actor Receiver { get; set; }

        protected DistributionReceiverStatusLogEnum State {get;set;}

        public SendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, DbUnitOfWork dbUnitOfWork)
            : base(repo, tableStorage, log, dbUnitOfWork)
        {
        }
        public virtual async Task PreExecuteAsync(SendQueueItem sendQueueItem)
        {
        }

        public virtual async Task PostExecuteAsync(SendQueueItem sendQueueItem)
        {
        }
        public virtual async Task<ReportQueueItem> ExecuteAsync(SendQueueItem sendQueueItem)
        {
            await base.LoadDataAsync(sendQueueItem.ArchiveReference);

            //this.Receiver = sendQueueItem.Receiver;

            //return new ReportQueueItem()
            //{
            //    ArchiveReference = sendQueueItem.ArchiveReference,
            //    ReceiverLogPartitionKey = sendQueueItem.ReceiverLogPartitionKey,
            //    ReceiverSequenceNumber = sendQueueItem.ReceiverSequenceNumber,
            //    Sender = sendQueueItem.Sender,
            //    Receiver = sendQueueItem.Receiver
            //};
            return null;

        }

        protected override async Task AddToReceiverProcessLogAsync(string receiverPartitionKey, string receiverID, DistributionReceiverStatusLogEnum statusEnum)
        {
            this.State = statusEnum;
            await base.AddToReceiverProcessLogAsync(receiverPartitionKey, receiverID, statusEnum);
        }

        protected override Task UpdateReceiverProcessStageAsync(string archiveReference, string receiverSequenceNumber, string receiverID, DistributionReceiverProcessStageEnum processStageEnum)
        {
            var task = base.UpdateReceiverProcessStageAsync(archiveReference, receiverSequenceNumber, receiverID, processStageEnum);
            
            return task;
        }
    }
}
