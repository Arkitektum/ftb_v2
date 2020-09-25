using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FtB_Sender.Strategies
{
    public abstract class SendStrategyBase : StrategyBase, IStrategy<ReportQueueItem, SendQueueItem>
    {
        private readonly ITableStorage _tableStorage;

        public SendStrategyBase(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public abstract void GetFormsAndAttachmentsFromBlobStorage();

        public virtual ReportQueueItem Exceute(SendQueueItem sendQueueItem)
        {
            FormLogicBeingProcessed.InitiateForm();
            return new ReportQueueItem() { ArchiveReference = sendQueueItem.ArchiveReference, Receiver = sendQueueItem.Receiver };
        }
    }
}