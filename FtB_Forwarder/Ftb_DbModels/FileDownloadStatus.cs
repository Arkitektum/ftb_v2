using FtB_Common.Enums;
using System;

namespace Ftb_DbModels
{
    public class FileDownloadStatus
    {
        public int Id { get; set; }
        public string ArchiveReference { get; set; }
        public Guid Guid { get; set; }
        public string FormName { get; set; }
        public FileTypesForDownloadEnum FileType { get; set; }
        public string Filename { get; set; }
        public string BlobLink { get; set; }
        public string MimeType { get; set; }
        public int FileAccessCount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? TimeReceived { get; set; }
        public FileDownloadStatus()
        { }
        public FileDownloadStatus(string archiveReference, Guid guid, FileTypesForDownloadEnum fileType, string fileName, string blobLink, string mimeType, string formName)
            : this(archiveReference, guid, fileType, fileName, blobLink, mimeType, formName, null)
        {
        }

        public FileDownloadStatus(string archiveReference, Guid guid, FileTypesForDownloadEnum fileType, string fileName, string blobLink, string mimeType, string formName, DateTime? timeReceived)
        {
            ArchiveReference = archiveReference;
            Guid = guid;
            FormName = formName;
            FileType = fileType;
            Filename = fileName;
            BlobLink = blobLink;
            MimeType = mimeType;
            TimeReceived = timeReceived;
        }    
    }
}
