using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_PrepareSending.Strategies
{
    public class DefaultDistributionPrepareStrategy : PrepareStrategyBase
    {
        public DefaultDistributionPrepareStrategy(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage) { }
        
         public override List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            return base.Exceute(submittalQueueItem);
        }
    }
}
