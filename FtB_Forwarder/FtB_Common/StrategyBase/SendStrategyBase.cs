using FtB_Common.Interfaces;

namespace FtB_Common
{
    public abstract class SendStrategyBase : IStrategy
    {
        IForm _formBeingProcessed;
        public SendStrategyBase(IForm form)
        {
            _formBeingProcessed = form;
        }

        public abstract void Exceute();

        public abstract void ForwardToReceiver();
        public abstract void GetFormsAndAttachmentsFromBlobStorage();
        public void ProcessForm()
        {
            //_formBeingProcessed.ProcessSendStep();
        }
    }
}