using FtB_Common;
using FtB_Common.Interfaces;

namespace FtB_Sender
{
    public abstract class SendStrategyBase : StrategyBase, IStrategy
    {
        public SendStrategyBase(IForm form)
        {
            _formBeingProcessed = form;
        }

        public abstract void Exceute();

        public abstract void ForwardToReceiver();
        public abstract void GetFormsAndAttachmentsFromBlobStorage();
    }
}