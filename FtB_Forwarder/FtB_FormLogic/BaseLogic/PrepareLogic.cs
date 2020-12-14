using FtB_Common;
using FtB_Common.BusinessLogic;
using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public abstract class PrepareLogic<T> : LogicBase<T>, IFormLogic<IEnumerable<SendQueueItem>, SubmittalQueueItem>
    {
        private readonly IDecryptionFactory _decryptionFactory;
        protected Actor Sender { get; set; }
        protected List<Actor> _receivers;
        protected virtual List<Actor> Receivers
        {
            //get { return _receivers.Distinct(new ReceiverEqualtiyComparer(_decryptionFactory)).ToList(); }
            get { return _receivers; }
            set { _receivers = value; }
        }

        public PrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, DbUnitOfWork dbUnitOfWork, IDecryptionFactory decryptionFactory)
            : base(repo, tableStorage, log, dbUnitOfWork)
        {
            _decryptionFactory = decryptionFactory;
        }

        public virtual void SetReceivers()
        { }

        public virtual void SetSender()
        { }

        public void SetReceivers(IEnumerable<Actor> receivers)
        {
            var comparrisonSource = new List<ActorInternal>();
            foreach (var receiver in receivers)
            {
                var receiverInternal = new ActorInternal(receiver);
                receiverInternal.DecryptedId = receiverInternal.Id.Length > 11 ? _decryptionFactory.GetDecryptor().DecryptText(receiverInternal.Id) : receiver.Id;
                comparrisonSource.Add(receiverInternal);
            }
            //_receivers = comparrisonSource.Distinct(new ReceiverEqualtiyComparer(_decryptionFactory)).ToList<Receiver>();
            var distinctList = comparrisonSource.Distinct(new ReceiverEqualtiyComparer()).ToList();
            _receivers = distinctList.Select(s => new Actor() { Id = s.Id, Type = s.Type }).ToList();
        }
        public virtual async Task PreExecuteAsync(SubmittalQueueItem submittalQueueItem)
        {
        }

        public virtual async Task PostExecuteAsync(SubmittalQueueItem submittalQueueItem)
        {
        }

        public virtual async Task<IEnumerable<SendQueueItem>> ExecuteAsync(SubmittalQueueItem submittalQueueItem)
        {
            _log.LogDebug($"{GetType().Name}: PreExecuteAsync: Processing logic for archveReference {submittalQueueItem.ArchiveReference}....");
            await base.LoadDataAsync(submittalQueueItem.ArchiveReference);
            SetSender();
            SetReceivers();
            return null;
        }

        
    }
}
