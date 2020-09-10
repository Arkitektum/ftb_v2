using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_CommonModel.Factories
{
    public abstract class AbstractProcessStepFactory
    {
        public abstract PrepareForwarding CreatePrepareForwarding(Forms.Form form);
        public abstract ExceuteForwarding CreateExceuteForwarding(Forms.Form form);
        public abstract ReportForwarding CreateReportForwarding();
    }
}
