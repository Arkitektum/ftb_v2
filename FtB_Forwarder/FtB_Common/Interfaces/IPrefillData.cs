using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Interfaces
{
    public interface IPrefillData
    {
        string DataFormatId { get; }
        string DataFormatVersion { get; }
        string PrefillFormName { get; }
        string InitialExternalSystemReference { get; set; }
        string ExternalSystemReference { get; }
        string PrefillServiceCode { get; }
        string PrefillServiceEditionCode { get; }
    }
}
