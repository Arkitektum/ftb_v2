using System;

namespace Altinn2.Adapters.Models
{
    public interface IArchivedItemMetadata
    {
        DateTime ArchivedDate { get; set; }
        string ArchiveReference { get; set; }
        string ReporteeId { get; }
        string ReporteeID { get; set; }
        DownloadQueueReportee ReporteeType { get; set; }
        string ServiceCode { get; set; }
        int ServiceEditionCode { get; set; }
    }
}
