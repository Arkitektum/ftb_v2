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
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the DistributionDefaultPrepareStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultDistributionPrepareStrategy(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage) { }
        
         public override List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            base.Exceute(submittalQueueItem);
            List<SendQueueItem> sendQueueItems = new List<SendQueueItem>();
            foreach (var receiverVar in Receivers)
            {
                var receiver = new Receiver() { Type = receiverVar.Type, Id = receiverVar.Id };
                sendQueueItems.Add(new SendQueueItem() { ArchiveReference = ArchiveReference,  Receiver = receiver } );
            }

            return sendQueueItems;
        }
    }
}
