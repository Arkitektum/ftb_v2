﻿using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_PrepareSending.Strategies
{
    public class DefaultShipmentPrepareStrategy : PrepareStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the ShipmentDefaultPrepareStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultShipmentPrepareStrategy(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage) { }

        public override List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            FormLogicBeingProcessed.ProcessPrepareStep();
            return null;
        }
    }
}
