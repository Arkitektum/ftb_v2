using FtB_Common.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Interfaces
{
    /// <summary>
    /// All form data needed in processing of the form, needs to be declared here....
    /// </summary>
    public interface IFormLogic
    {
        void ProcessPrepareStep();
        void ProcessSendStep(string filter);
        void ProcessReportStep();
        void LoadFormData(string archiveReference);
        string ArchiveReference { get; set; }
        List<Receiver> Receivers { get; set; }
        string DistributionData { get; set; }
        void InitiateForm();
        PrefillData GetPrefillData(string filter, string identifier);
        //IFormDataValidator GetFormDataValidator();
    }
}
