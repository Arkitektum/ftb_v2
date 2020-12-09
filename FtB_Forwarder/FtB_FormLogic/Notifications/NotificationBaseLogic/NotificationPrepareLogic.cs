using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
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
            return await base.ExecuteAsync(submittalQueueItem);
        }
    }
}
