using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System.Collections.Generic;

namespace FtB_Sender.Strategies
{
    public abstract class SendStrategyBase : StrategyBase, IStrategy<ReportQueueItem, SendQueueItem>
    {
        private readonly ITableStorage _tableStorage;

        public SendStrategyBase(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public abstract void ForwardToReceiver();
        public abstract void GetFormsAndAttachmentsFromBlobStorage();

        //private int GetSubmittalReceiverCount(string archiveReference)
        //{
        //    var table = _tableStorage.GetTableEntity("ftbSubmittals", archiveReference, archiveReference);
        //    var submittalEntity = (SubmittalEntity)table.Result.Result;
        //    return submittalEntity.ReceiverCount;
        //}


        private bool AllReceiversHasBeenSentTo(string archiveReference)
        {
            var table = _tableStorage.GetTableEntity("ftbSubmittals", archiveReference, archiveReference);
            var submittalEntity = (SubmittalEntity)table.Result.Result;
            return submittalEntity.ReceiverCount == submittalEntity.SentCount;
        }

        private void IncrementSubmittalSentCount(string archiveReference)
        {
            var table = _tableStorage.GetTableEntity("ftbSubmittals", archiveReference, archiveReference);
            var submittalEntity = (SubmittalEntity)table.Result.Result;
            submittalEntity.SentCount++;
            _tableStorage.InsertSubmittalRecord(submittalEntity, "ftbSubmittals");
        }

        public virtual List<ReportQueueItem> Exceute(SendQueueItem sendQueueItem)
        {
            FormLogicBeingProcessed.InitiateForm();
            //SetReceivers();
            IncrementSubmittalSentCount(sendQueueItem.ArchiveReference);
            if (AllReceiversHasBeenSentTo(sendQueueItem.ArchiveReference))
            {
                return new List<ReportQueueItem>()
                {
                    new ReportQueueItem() { ArchiveReference = sendQueueItem.ArchiveReference, Receivers = FormLogicBeingProcessed.Receivers }
                };
            }
            return null;
        }

    }
}