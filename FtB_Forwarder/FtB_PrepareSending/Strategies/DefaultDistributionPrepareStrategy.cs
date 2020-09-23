﻿using FtB_Common;
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
        public DefaultDistributionPrepareStrategy(IFormLogic formLogic) : base(formLogic) { }
        
        protected override void CreateSubmittalDatabaseStatus(string archiveReference)
        {
            Console.WriteLine("Oppretter databasestatus for DISTRIBUTION");
        }

        public override List<SendQueueItem> Exceute()
        {
            base.Exceute();
            List<SendQueueItem> sendQueueItems = new List<SendQueueItem>();
            foreach (var receiver in Receivers)
            {

                sendQueueItems.Add(new SendQueueItem() { ArchiveReference = ArchiveReference, ReceiverType = receiver.Type.ToString() , ReceiverId = receiver.Id });
            }

            return sendQueueItems;
        }
    }
}
