using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic
{
    public class ReportLogic<T> : LogicBase<T>, IFormLogic<FinishedQueueItem, ReportQueueItem>
    {
        protected virtual Receiver Receiver { get; set; }

        public ReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log) : base(repo, tableStorage, log)
        {

        }

        public virtual FinishedQueueItem Execute(ReportQueueItem reportQueueItem)
        {
            return new FinishedQueueItem() { ArchiveReference = reportQueueItem.ArchiveReference };
        }
    }
}

