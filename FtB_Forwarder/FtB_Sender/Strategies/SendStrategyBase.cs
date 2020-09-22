using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System.Collections.Generic;

namespace FtB_Sender.Strategies
{
    public abstract class SendStrategyBase : StrategyBase, IStrategy<ReportQueueItem>
    {
        public SendStrategyBase(IForm form)
        {
            _formBeingProcessed = form;
        }


        public abstract void ForwardToReceiver();
        public abstract void GetFormsAndAttachmentsFromBlobStorage();

        public abstract List<ReportQueueItem> Exceute();
    }
}