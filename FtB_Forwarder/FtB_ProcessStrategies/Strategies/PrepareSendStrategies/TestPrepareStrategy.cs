﻿using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public class TestPrepareStrategy : PrepareStrategyBase
    {
        public TestPrepareStrategy(IFormLogic formLogic, ITableStorage tableStorage, ILogger log) : base(formLogic, tableStorage, log) { }
        
         public override List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            MultipleUpTheReceiversForTheStrategy();

            return base.Exceute(submittalQueueItem);
        }
    }
}
