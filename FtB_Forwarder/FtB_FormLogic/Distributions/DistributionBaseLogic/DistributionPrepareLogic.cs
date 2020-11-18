using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FtB_FormLogic
{
    public abstract class DistributionPrepareLogic<T> : PrepareLogic<T>
    {
        private readonly ILogger log;
        protected  override  List<Receiver> Receivers { get => base.Receivers; set => base.Receivers = value; }

        public DistributionPrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ITableStorageOperations tableStorageOperations, ILogger log, DbUnitOfWork dbUnitOfWork, IDecryptionFactory decryptionFactory) 
            : base(repo, tableStorage, tableStorageOperations, log, dbUnitOfWork, decryptionFactory)
        {
            this.log = log;
        }

        public override IEnumerable<SendQueueItem> Execute(SubmittalQueueItem submittalQueueItem)
        {
            return base.Execute(submittalQueueItem);
        }
    }
}
