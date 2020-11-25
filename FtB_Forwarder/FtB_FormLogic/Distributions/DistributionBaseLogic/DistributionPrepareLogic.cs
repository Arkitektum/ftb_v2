using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public abstract class DistributionPrepareLogic<T> : PrepareLogic<T>
    {
        private readonly ILogger log;
        protected  override  List<Receiver> Receivers { get => base.Receivers; set => base.Receivers = value; }

        public DistributionPrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, DbUnitOfWork dbUnitOfWork, IDecryptionFactory decryptionFactory) 
            : base(repo, tableStorage, log, dbUnitOfWork, decryptionFactory)
        {
            this.log = log;
        }

        public override async Task<IEnumerable<SendQueueItem>> Execute(SubmittalQueueItem submittalQueueItem)
        {
            return await base.Execute(submittalQueueItem);
        }
    }
}
