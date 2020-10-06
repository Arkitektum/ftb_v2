using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic
{
    public class ReportLogic<T> : LogicBase<T>, IFormLogic<string, ReportQueueItem>
    {
        protected virtual Receiver Receiver { get; set; }

        public ReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log) : base(repo, tableStorage, log)
        {

        }

        public virtual string Execute(ReportQueueItem reportQueueItem)
        {
            return "";
        }
    }
}

