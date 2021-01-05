using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public abstract class DistributionPrepareLogic<T> : PrepareLogic<T>
    {
        private readonly ILogger log;
        protected  override  List<Actor> Receivers { get => base.Receivers; set => base.Receivers = value; }

        public DistributionPrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, DbUnitOfWork dbUnitOfWork, IDecryptionFactory decryptionFactory) 
            : base(repo, tableStorage, log, dbUnitOfWork, decryptionFactory)
        {
            this.log = log;
        }

        public override async Task<IEnumerable<SendQueueItem>> ExecuteAsync(SubmittalQueueItem submittalQueueItem)
        {
            var returnValue = await base.ExecuteAsync(submittalQueueItem);
            
            await CreateDistributionSubmittalDatabaseStatus(submittalQueueItem.ArchiveReference, Sender.Id, Receivers.Count);
            var sendQueueItems = await CreateDistributionSendQueueItems(submittalQueueItem);

            return sendQueueItems;
        }


        private async Task<IEnumerable<SendQueueItem>> CreateDistributionSendQueueItems(SubmittalQueueItem submittalQueueItem)
        {
            var sendQueueItems = new List<SendQueueItem>();

            //Bulk add receivers to database
            //FUN FACT: Since the partition key differs for all receivers a true bulk operation cannot be performed..
            var receiverEntities = new List<DistributionReceiverEntity>();
            var receiverLogEntities = new List<DistributionReceiverLogEntity>();
            for (int i = 0; i < Receivers.Count; i++)
            {
                string receiverLogPartitionKey = $"{ArchiveReference}-{i.ToString()}";
                string rowKey = $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}";

                receiverEntities.Add(new DistributionReceiverEntity(ArchiveReference, i.ToString(), Receivers[i].Id, DistributionReceiverProcessStageEnum.Created, DateTime.Now, receiverLogPartitionKey));
                receiverLogEntities.Add(new DistributionReceiverLogEntity(receiverLogPartitionKey, rowKey, Receivers[i].Id, DistributionReceiverStatusLogEnum.Created));

                sendQueueItems.Add(new SendQueueItem()
                {
                    ArchiveReference = ArchiveReference,
                    ReceiverSequenceNumber = i.ToString(),
                    ReceiverLogPartitionKey = receiverLogPartitionKey,
                    Receiver = Receivers[i],
                    Sender = Sender
                });
            }

            _log.LogDebug($"Created {receiverEntities.Count} receiver entities for {submittalQueueItem.ArchiveReference}");

            await BulkInsertEntitiesAsync(receiverEntities);
            await ParallelInsertEntitiesAsync(receiverLogEntities);

            return sendQueueItems;
        }
        protected virtual async Task CreateDistributionSubmittalDatabaseStatus(string archiveReference, string senderId, int receiverCount)
        {
            throw new NotImplementedException();
        }

    }
}
