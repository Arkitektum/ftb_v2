using Altinn.Common;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    public abstract class DistributionSendLogic<T> :  SendLogic<T>
    {
        private readonly IDistributionAdapter _distributionAdapter;
        
        public AltinnDistributionMessage DistributionMessage { get; set; }       


        public DistributionSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, IDistributionAdapter distributionAdapter) : base(repo, tableStorage, log)
        {
            _distributionAdapter = distributionAdapter;
        }

        public override ReportQueueItem Execute(SendQueueItem sendQueueItem)
        {
            var returnReportQueueItem = base.Execute(sendQueueItem);
            MapPrefillData(sendQueueItem.Receiver.Id);
            UpdateReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.PrefillCreated);

            //prefillData = 
            PersistPrefill(sendQueueItem);

            var result = _distributionAdapter.SendDistribution(DistributionMessage);

            //Use result of SendDistribution to update receiver entity
            UpdateReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.CorrespondenceSent);

            return returnReportQueueItem;
        }

        protected virtual void PersistPrefill(SendQueueItem sendQueueItem)
        {
            var metaData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("PrefillReceiver", sendQueueItem.Receiver.Id) };
            _log.LogDebug($"{GetType().Name}: PersistPrefill for archiveReference {sendQueueItem.ArchiveReference}....");
            _repo.AddBytesAsBlob(sendQueueItem.ArchiveReference, $"Prefill-{Guid.NewGuid()}", Encoding.Default.GetBytes(DistributionMessage.PrefilledXmlDataString), metaData);
            UpdateReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.PrefillPersisted);
        }

        protected virtual void PersistMessage(SendQueueItem sendQueueItem)
        {
            var metaData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("DistributionMessageReceiver", sendQueueItem.Receiver.Id) };
            _log.LogDebug($"{GetType().Name}: PersistMessage for archiveReference {sendQueueItem.ArchiveReference}....");
            _repo.AddBytesAsBlob(sendQueueItem.ArchiveReference, $"Message-{Guid.NewGuid()}", Encoding.Default.GetBytes(SerializeUtil.Serialize(DistributionMessage.NotificationMessage.MessageData)), metaData);
            UpdateReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.PrefillPersisted);
        }

        //protected virtual IEnumerable<AltinnDistributionResult> SendPrefill(SendQueueItem sendQueueItem)
        //{
        //    var distributionResults = _distributionAdapter.SendDistribution(DistributionMessage);
        //    //switch (prefillResult.ResultType)
        //    //{
        //    //    case PrefillResultType.Ok:
        //    //        UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.PrefillSent));
        //    //        break;
        //    //    case PrefillResultType.UnkownErrorOccured:
        //    //        break;
        //    //    case PrefillResultType.ReservedReportee:
        //    //        break;
        //    //    case PrefillResultType.UnableToReachReceiver:
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}

        //    return distributionResults;
        //}

        //protected virtual void Distribute(SendQueueItem sendQueueItem, string prefillReferenceId)
        //{
        //    // Validate if receiver info is sufficient

        //    // Decrypt

        //    // Create distributionform 
        //    var distributionFormId = this.PrefillData.DistributionFormId;

        //    // Map  from prefill-data to prefillFormTask
        //    // Send using prefill service

        //    // _distributionAdapter.SendDistribution(eitEllerAnnaObjekt);

        //    // Finally persist distributionform..  and maybe a list of logentries??            

        //    UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.CorrespondenceSent));
        //}
        protected abstract void MapPrefillData(string receiverId);
    }
}

