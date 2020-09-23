﻿using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Reporter.Strategies
{
    public class DefaultNotificationReportStrategy : ReportStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the NotificationDefaultReportStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultNotificationReportStrategy(IForm form) : base(form) { }

        public override List<FinishedQueueItem> Exceute()
        {
            _formBeingProcessed.ProcessReportStep();
            return null;
        }
    }
}
