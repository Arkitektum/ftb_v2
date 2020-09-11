using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_CommonModel.Factories
{
    public abstract class AbstractChannelFactory
    {
        public abstract PrepareStrategyBase CreatePrepareBase(Forms.FormBase form);
        public abstract SendStrategyBase CreateSendBase(Forms.FormBase form);
        public abstract ReportStrategyBase CreateReportBase();
    }
}
