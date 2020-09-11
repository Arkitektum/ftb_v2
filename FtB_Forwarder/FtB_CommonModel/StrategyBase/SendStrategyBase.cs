using FtB_CommonModel.Forms;
using FtB_CommonModel.Interfaces;

namespace FtB_CommonModel.Models
{
    public abstract class SendStrategyBase : IProcess
    {
        FormBase _formBeingProcessed;
        public SendStrategyBase(FormBase form)
        {
            _formBeingProcessed = form;
        }

        public void Exceute()
        {
            throw new System.NotImplementedException();
        }

        public abstract void ForwardToReceiver();
        public abstract void GetFormsAndAttachmentsFromBlobStorage();
        public void ProcessForm()
        {
            _formBeingProcessed.ProcessSendStep();
        }
    }
}