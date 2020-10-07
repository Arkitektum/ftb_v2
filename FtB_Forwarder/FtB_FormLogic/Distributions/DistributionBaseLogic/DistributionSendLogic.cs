using FtB_Common.Adapters;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    public abstract class DistributionSendLogic<T> :  SendLogic<T>
    {
        private readonly IPrefillAdapter _prefillAdapter;

        public PrefillData PrefillData { get; set; }
        protected string prefillDataString { get; set; }


        public DistributionSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, IPrefillAdapter prefillAdapter) : base(repo, tableStorage, log)
        {
            _prefillAdapter = prefillAdapter;
        }

        public override ReportQueueItem Execute(SendQueueItem sendQueueItem)
        {
            var returnReportQueueItem = base.Execute(sendQueueItem);

            MapPrefillData(sendQueueItem.Receiver.Id);            
            UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.PrefillCreated));

            PersistPrefill(sendQueueItem);

            var result = SendPrefill(sendQueueItem);

            Distribute(sendQueueItem, result.PrefillReferenceId);

            return returnReportQueueItem;
        }

        protected virtual void PersistPrefill(SendQueueItem sendQueueItem)
        {
            var metaData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("PrefillReceiver", sendQueueItem.Receiver.Id) };
            _repo.AddBytesAsBlob(sendQueueItem.ArchiveReference, $"Prefill-{Guid.NewGuid()}", Encoding.Default.GetBytes(PrefillData.XmlDataString), metaData);
            UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.PrefillPersisted));
        }

        protected virtual PrefillResult SendPrefill(SendQueueItem sendQueueItem)
        {
            var prefillResult = _prefillAdapter.SendPrefill(PrefillData);            
            switch (prefillResult.ResultType)
            {
                case PrefillResultType.Ok:
                    UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.PrefillSent));
                    break;
                case PrefillResultType.UnkownErrorOccured:
                    break;
                case PrefillResultType.ReservedReportee:
                    break;
                case PrefillResultType.UnableToReachReceiver:
                    break;
                default:
                    break;
            }
            return prefillResult;
        }

        protected virtual void Distribute(SendQueueItem sendQueueItem, string prefillReference)
        {
            // Validate if receiver info is sufficient

            // Decrypt

            // Create distributionform 
            var distributionFormId = this.PrefillData.DistributionFormId;

            // Map  from prefill-data to prefillFormTask
            // Send using prefill service

            // _distributionAdapter.SendDistribution(eitEllerAnnaObjekt);

            // Finally persist distributionform..  and maybe a list of logentries??            

            UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.CorrespondenceSent));
        }
        protected abstract void MapPrefillData(string receiverId);
    }
}
