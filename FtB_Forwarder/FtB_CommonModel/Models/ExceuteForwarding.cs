namespace FtB_CommonModel.Models
{
    public abstract class ExceuteForwarding
    {
        public abstract void ForwardToReceiver();
        public abstract void GetFormsAndAttachmentsFromBlobStorage();
    }
}