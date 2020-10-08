using System;

namespace Altinn2.Adapters.Models
{
    /// <summary>
    /// Abstraction of the Altinn download queue type
    /// </summary>
    public class ArchivedItemMetadata : IArchivedItemMetadata
    {
        public string ArchiveReference { get; set; }

        public DateTime ArchivedDate { get; set; }
        public string ReporteeId { get; }
        public string ReporteeID { get; set; }

        public DownloadQueueReportee ReporteeType { get; set; }

        public string ServiceCode { get; set; }

        public int ServiceEditionCode { get; set; }

        public ArchivedItemMetadata(string archiveReference, DateTime archivedDate, string reporteeId, string reporteeType, string serviceCode, int serviceEditionCode)
        {
            ArchiveReference = archiveReference;
            ArchivedDate = archivedDate;
            ReporteeId = reporteeId;
            ReporteeType = (DownloadQueueReportee)Enum.Parse(typeof(DownloadQueueReportee), reporteeType);
            ServiceCode = serviceCode;
            ServiceEditionCode = serviceEditionCode;
        }
    }
}