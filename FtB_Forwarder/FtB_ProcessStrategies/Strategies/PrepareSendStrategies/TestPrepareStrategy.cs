using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public class TestPrepareStrategy : PrepareStrategyBase
    {
        public TestPrepareStrategy(ITableStorage tableStorage, ILogger<TestPrepareStrategy> log) : base(tableStorage, log) { }
        
         public override List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            MultipleUpTheReceiversForTheStrategy();

            return base.Exceute(submittalQueueItem);
        }
    }
}
