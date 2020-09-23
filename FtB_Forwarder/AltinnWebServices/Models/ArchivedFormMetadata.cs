using System;

namespace AltinnWebServices.Models
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

namespace AltinnWebServices
{
    public enum DownloadQueueReportee
    {
        Organisation,
        Person,
        SelfRegisteredUser
    }
}
