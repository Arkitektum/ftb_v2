using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System.Collections.Generic;

namespace FtB_Sender.Strategies
{
    public abstract class SendStrategyBase : StrategyBase, IStrategy<ReportQueueItem>
    {
        public SendStrategyBase(IFormLogic formLogic) : base(formLogic)
        {
        }

        public abstract void ForwardToReceiver();
        public abstract void GetFormsAndAttachmentsFromBlobStorage();

        public virtual List<ReportQueueItem> Exceute()
        {
            return null;
             //_formBeingProcessed.ReceiverIdentifers.Add("Ole Brumm");
        }
    }
}