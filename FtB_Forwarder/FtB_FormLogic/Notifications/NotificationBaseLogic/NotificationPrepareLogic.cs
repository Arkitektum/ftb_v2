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
            await CreateNotificationReceiverDatabaseStatus(submittalQueueItem.ArchiveReference, Sender.Id);
            
            var sendQueueItems = await CreateNotificationSendQueueItem(submittalQueueItem);

            var queueList = new List<SendQueueItem>();
            queueList.Add(sendQueueItems);

            return queueList;
        }

        private async Task<SendQueueItem> CreateNotificationSendQueueItem(SubmittalQueueItem submittalQueueItem)
        {
            var sendQueueItems = new List<SendQueueItem>();
            string rowKey = $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}";

            return new SendQueueItem()
            {
                ArchiveReference = ArchiveReference,
                ReceiverSequenceNumber = "0",
                Receiver = Receivers[0],
                Sender = Sender
            };
        }

        protected virtual async Task CreateNotificationReceiverDatabaseStatus(string archiveReference, string senderId)
        { 
            throw new Exception("Not to be implemented"); 
        }

    }
}