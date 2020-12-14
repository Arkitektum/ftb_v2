using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public class NotificationPrepareLogic<T> : PrepareLogic<T>
    {
        public NotificationPrepareLogic(IFormDataRepo repo,
                                        ITableStorage tableStorage,
                                        ILogger log,
                                        DbUnitOfWork dbUnitOfWork,
                                        IDecryptionFactory decryptionFactory)
            : base(repo, tableStorage, log, dbUnitOfWork, decryptionFactory)
        { }
        public override async Task<IEnumerable<SendQueueItem>> ExecuteAsync(SubmittalQueueItem submittalQueueItem)
        {
            var returnValue = await base.ExecuteAsync(submittalQueueItem);
            await CreateNotificationSubmittalDatabaseStatus(submittalQueueItem.ArchiveReference, Sender.Id);
            var sendQueueItems = await CreateNotificationSendQueueItem(submittalQueueItem);

            var queueList = new List<SendQueueItem>();
            queueList.Add(sendQueueItems);

            return queueList;
        }

        private async Task CreateNotificationSubmittalDatabaseStatus(string archiveReference, string senderId)
        {
            try
            {
                var entity = new NotificationSubmittalEntity(archiveReference, senderId, DateTime.Now);
                await _tableStorage.InsertEntityRecordAsync<NotificationSubmittalEntity>(entity);
                _log.LogDebug($"Create submittal database status for {archiveReference}.");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Error creating submittal record for archiveReference={archiveReference}.");
                throw;
            }
        }

        private async Task<SendQueueItem> CreateNotificationSendQueueItem(SubmittalQueueItem submittalQueueItem)
        {
            var sendQueueItems = new List<SendQueueItem>();
            string rowKey = $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}";

            var receiverEntity = new NotificationReceiverEntity(ArchiveReference, ArchiveReference, Receivers[0].Id, ReceiverProcessStageEnum.Created, DateTime.Now);
            await _tableStorage.InsertEntityRecordAsync<NotificationReceiverEntity>(receiverEntity);

            var receiverLogEntity = new NotificationReceiverLogEntity(ArchiveReference, rowKey, Receivers[0].Id, ReceiverStatusLogEnum.Created);
            await _tableStorage.InsertEntityRecordAsync<NotificationReceiverLogEntity>(receiverLogEntity);

            return new SendQueueItem()
            {
                ArchiveReference = ArchiveReference,
                ReceiverSequenceNumber = "0",
                Receiver = Receivers[0],
                Sender = Sender
            };
        }

    }
}