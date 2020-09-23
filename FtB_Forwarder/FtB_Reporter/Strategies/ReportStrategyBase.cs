﻿using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System.Collections.Generic;

namespace FtB_Reporter.Strategies
{
    public abstract class ReportStrategyBase : StrategyBase, IStrategy<FinishedQueueItem>
    {
        public ReportStrategyBase(IFormLogic form) : base(form)
        {
        }

        public abstract List<FinishedQueueItem> Exceute();
    }
}