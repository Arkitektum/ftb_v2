﻿using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public class DefaultNotificationPrepareStrategy : PrepareStrategyBase
    {
        public DefaultNotificationPrepareStrategy(ITableStorage tableStorage, ILogger<DefaultNotificationPrepareStrategy> log) : base(tableStorage, log) { }

        public override List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            return base.Exceute(submittalQueueItem);

        }
    }
}
