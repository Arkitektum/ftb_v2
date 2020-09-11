using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_CommonModel.Factories
{
    public abstract class AbstractChannelFactory
    {
        public abstract PrepareBase CreatePrepareBase(Forms.FormBase form);
        public abstract SendBase CreateSendBase(Forms.FormBase form);
        public abstract Reportbase CreateReportBase();
    }
}
