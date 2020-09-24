using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_PrepareSending.Strategies
{
    public class DefaultNotificationPrepareStrategy : PrepareStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the NotificationDefaultPrepareStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultNotificationPrepareStrategy(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage) { }

        public override List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            return base.Exceute(submittalQueueItem);

        }
    }
}
