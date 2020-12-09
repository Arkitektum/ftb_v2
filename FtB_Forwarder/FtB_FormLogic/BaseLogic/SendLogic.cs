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

        protected virtual Receiver Receiver { get; set; }

        protected ReceiverStatusLogEnum State {get;set;}

        public SendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, DbUnitOfWork dbUnitOfWork)
            : base(repo, tableStorage, log, dbUnitOfWork)
        {
        }

        public virtual async Task<ReportQueueItem> ExecuteAsync(SendQueueItem sendQueueItem)
        {
            this.Receiver = sendQueueItem.Receiver;

            //Check state
            this.State = await base.GetReceiverLastLogStatusAsync(sendQueueItem.ReceiverLogPartitionKey);

            if (this.State == ReceiverStatusLogEnum.Created 
                || this.State == ReceiverStatusLogEnum.PrefillCreated
                || this.State == ReceiverStatusLogEnum.PrefillSendingFailed
                || this.State == ReceiverStatusLogEnum.CorrespondenceSendingFailed)
            {
                await base.LoadDataAsync(sendQueueItem.ArchiveReference);

                return new ReportQueueItem()
                {
                    ArchiveReference = sendQueueItem.ArchiveReference,
                    ReceiverLogPartitionKey = sendQueueItem.ReceiverLogPartitionKey,
                    ReceiverSequenceNumber = sendQueueItem.ReceiverSequenceNumber,
                    Receiver = sendQueueItem.Receiver
                };
            }
            return null;
        }

        protected override async Task AddToReceiverProcessLogAsync(string receiverPartitionKey, string receiverID, ReceiverStatusLogEnum statusEnum)
        {
            this.State = statusEnum;
            await base.AddToReceiverProcessLogAsync(receiverPartitionKey, receiverID, statusEnum);
        }

        protected override Task UpdateReceiverProcessStageAsync(string archiveReference, string receiverSequenceNumber, string receiverID, ReceiverProcessStageEnum processStageEnum)
        {
            var task = base.UpdateReceiverProcessStageAsync(archiveReference, receiverSequenceNumber, receiverID, processStageEnum);
            
            return task;
        }
    }
}
