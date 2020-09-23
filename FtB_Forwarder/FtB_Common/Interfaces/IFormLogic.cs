using FtB_Common.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Interfaces
{
    public interface IFormLogic
    {
        void ProcessPrepareStep();
        void ProcessSendStep();
        void ProcessReportStep();
        void LoadFormData(string archiveReference);
        string ArchiveReference { get; set; }
        List<Receiver> Receivers { get; set; }
        void InitiateForm();
    }
}
