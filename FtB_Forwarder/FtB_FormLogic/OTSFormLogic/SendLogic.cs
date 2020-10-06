using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic.OTSFormLogic
{
    public abstract class SendLogic<T> : LogicBase<T>, IFormLogic<ReportQueueItem, SendQueueItem>
    {
        protected virtual Receiver Receiver { get; set; }

        public SendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log) : base(repo, tableStorage, log)
        {
        }

        public virtual ReportQueueItem Execute(SendQueueItem input)
        {

            return null;
        
        }
    }
}
