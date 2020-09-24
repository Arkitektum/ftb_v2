using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System.Collections.Generic;

namespace FtB_Reporter.Strategies
{
    public abstract class ReportStrategyBase : StrategyBase, IStrategy<FinishedQueueItem, ReportQueueItem>
    {
        public ReportStrategyBase(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage)
        {
        }

        public abstract List<FinishedQueueItem> Exceute(ReportQueueItem reportQueueItem);

    }
}