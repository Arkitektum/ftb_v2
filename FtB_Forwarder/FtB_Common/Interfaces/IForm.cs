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
        void LoadFormData(string archiveReference);
        string ArchiveReference { get; set; }
        List<string> ReceiverIdentifers { get; set; }
        void InitiateForm();
    }
}
