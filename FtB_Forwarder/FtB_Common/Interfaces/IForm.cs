using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Interfaces
{
    public interface IForm
    {
        void ProcessPrepareStep();
        void ProcessSendStep();
        void ProcessReportStep();
        string GetFormatId();
        void LoadFormData(string archiveReference);
    }
}
