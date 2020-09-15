using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Interfaces
{
    public interface IForm
    {
        IStrategy GetCustomizedPrepareStrategy();
        IStrategy GetCustomizedSendStrategy();
        IStrategy GetCustomizedReportStrategy();
        void ProcessPrepareStep();
        void ProcessSendStep();
        void ProcessReportStep();
        string GetFormatId();
    }
}
