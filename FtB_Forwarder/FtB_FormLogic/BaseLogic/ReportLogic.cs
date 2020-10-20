using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Ftb_DbRepository;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic
{
    public class ReportLogic<T> : LogicBase<T>, IFormLogic<string, ReportQueueItem>, IReportLogic
    {
        public ReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, DbUnitOfWork dbUnitOfWork) : base(repo, tableStorage, log, dbUnitOfWork)
        {
        }

        public virtual void SetSubmitterReportContent(SubmittalEntity submittalEntity)
        {
        }

        public virtual string Execute(ReportQueueItem reportQueueItem)
        {
            base.LoadData(reportQueueItem.ArchiveReference);

            return reportQueueItem.ArchiveReference;
        }
    }
}

